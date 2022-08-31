using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.GameObjects.Available;
using CamelUpEngine.GameTools;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine
{
    public sealed class Game
    {
        public const int MinimalPlayersCount = 3;
        public const int MaximalPlayersCount = 8;
        public const int DefaultFieldsCount = 16;
        public const int MaximalDrawnDices = 5;

        private Player currentPlayer;
        private readonly List<Player> players;
        private readonly List<Field> fields;
        private readonly Dicer dicer;
        private readonly BetCardsManager betManager;
        private readonly TypingCardsManager cardManager;
        private readonly CamelTrafficManager camelsManager;
        private readonly AudienceTilesManager tilesManager;

        public ActionEvents History => ActionEventsCollector.GetGameEvents();
        public IPlayer CurrentPlayer => currentPlayer;
        public IReadOnlyCollection<IPlayer> Players => players.ToList();
        public IReadOnlyCollection<IField> Fields => fields.ToList();
        public IReadOnlyCollection<ICamel> Camels => camelsManager.OrderedAllCamels;
        public IReadOnlyCollection<IDrawnDice> DrawnDices => dicer.DrawnDices;
        public IReadOnlyCollection<IAvailableField> AudienceTileAvailableFields => tilesManager.GetAudienceTileAvailableFields();
        public IReadOnlyCollection<IAvailableBetCard> AvailableBetCards => betManager.GetPlayerAvailableBetCards(CurrentPlayer);
        public IReadOnlyCollection<IAvailableTypingCard> AvailableTypingCards => cardManager.AvailableCards;
        public IReadOnlyCollection<IBetCard> WinnerBets => GameIsOver ? betManager.WinnerBetsStack : null;
        public IReadOnlyCollection<IBetCard> LoserBets => GameIsOver ? betManager.LoserBetsStack : null;

        public bool GameIsOver => camelsManager.AnyCamelPassFinishLine;
        public bool TurnIsOver => dicer.DrawnDices.Count() >= MaximalDrawnDices;

        public Game(IEnumerable<string> playerNames, bool randomizePlayersOrder = false, int fieldsCount = DefaultFieldsCount)
        {
            ActionEventsCollector.Reset();
            fields = GameInitializer.GenerateFields(fieldsCount).ToList();
            players = GameInitializer.GeneratePlayers(playerNames, randomizePlayersOrder).ToList();
            GameInitializer.SetCamelsOnBoard(this);
            dicer = new();
            betManager = new(players);
            cardManager = new();
            tilesManager = new(fields);
            camelsManager = new(fields);
            currentPlayer = players.First();
        }

        public static bool IsPlayerNameValid(string playerName) => !GameInitializer.IsInvalidPlayerName(playerName);

        public ActionEvents DrawDice()
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (TurnIsOver)
            {
                throw new TurnIsOverException();
            }

            IDrawnDice drawnDice = dicer.DrawDice();
            ActionEventsCollector.AddEvent(new DiceDrawnEvent(drawnDice));
            ActionEventsCollector.AddEvent(new CoinsAddedEvent(currentPlayer, currentPlayer.AddCoins(1)));
            ActionEventsCollector.AddEvent(camelsManager.MoveCamel(drawnDice.Colour, drawnDice.Value));

            if (GameIsOver)
            {
                FinishGame();
                return ActionEventsCollector.GetActionEvents();
            }

            if (TurnIsOver)
            {
                ActionEventsCollector.AddEvent(new EndOfTurnEvent());
            }
            else
            {
                SetNextPlayer();
            }

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents MakeBet(IAvailableBetCard availableBetCard, BetType betType)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (TurnIsOver)
            {
                throw new TurnIsOverException();
            }

            ActionEventsCollector.AddEvent(betManager.MakeBet(CurrentPlayer, availableBetCard, betType));

            SetNextPlayer();

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents DrawTypingCard(IAvailableTypingCard card)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (TurnIsOver)
            {
                throw new TurnIsOverException();
            }

            TypingCard typingCard = (TypingCard)cardManager.DrawCard(card);
            currentPlayer.AddTypingCard(typingCard);
            ActionEventsCollector.AddEvent(new TypingCardDrawnEvent(currentPlayer, typingCard));

            SetNextPlayer();

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents PlaceAudienceTile(IAvailableField availableField, AudienceTileSide audienceTileSide)
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (TurnIsOver)
            {
                throw new TurnIsOverException();
            }

            var placementAudienceTileEvent = tilesManager.PlaceAudienceTile(currentPlayer, availableField, audienceTileSide, out var removementAudienceTileEvent);
            if (removementAudienceTileEvent != null)
            {
                ActionEventsCollector.AddEvent(removementAudienceTileEvent);
            }
            ActionEventsCollector.AddEvent(placementAudienceTileEvent);

            SetNextPlayer();

            return ActionEventsCollector.GetActionEvents();
        }

        public ActionEvents GoToNextTurn()
        {
            if (GameIsOver)
            {
                throw new GameOverException();
            }

            if (!TurnIsOver)
            {
                throw new TurnIsNotOverException();
            }

            RemoveAllAudienceTiles();
            RefillDicer();
            SummarizeCurrentTurn();

            ActionEventsCollector.AddEvent(new NewTurnEvent());

            SetNextPlayer();

            return ActionEventsCollector.GetActionEvents();
        }

        private void FinishGame()
        {
            ActionEventsCollector.AddEvent(new EndOfTurnEvent());
            SummarizeCurrentTurn();
            SummarizeBets();
            ActionEventsCollector.AddEvent(new GameOverEvent(this));
        }

        private void SummarizeCurrentTurn()
        {
            List<ICoinsAddedEvent> playerCoinsEarnedEvents = new();
            List<IPlayerTypingCardsReturnedEvent> playerTypingCardsReturnedEvents = new();

            foreach (Player player in players)
            {
                IReadOnlyCollection<ITypingCard> returnedTypingCard = player.ReturnTypingCards();
                if (returnedTypingCard.Any())
                {
                    playerTypingCardsReturnedEvents.Add(new PlayerTypingCardsReturnedEvent(player, returnedTypingCard));
                    int earnedCoins = TypingCardsManager.CountCoins(camelsManager.OrderedAllCamels, player.TypingCards);
                    if (earnedCoins != 0)
                    {
                        playerCoinsEarnedEvents.Add(new CoinsAddedEvent(player, player.AddCoins(earnedCoins)));
                    }
                }
            }
            ActionEventsCollector.AddEvent(new TypingCardsSummaryEvent(playerCoinsEarnedEvents));

            cardManager.Reset();
            ActionEventsCollector.AddEvent(new AllTypingCardsReturnedEvent(playerTypingCardsReturnedEvents));
        }

        private void SummarizeBets()
        {
            IList<ICoinsAddedEvent> winnerRewards = SummarizeSingleStackBets(camelsManager.OrderedCamels.First(), betManager.WinnerBetsStack);
            IList<ICoinsAddedEvent> loserRewards = SummarizeSingleStackBets(camelsManager.OrderedCamels.Last(), betManager.LoserBetsStack);
            ActionEventsCollector.AddEvent(new BetsSummaryEvent(winnerRewards, loserRewards));
        }

        private List<ICoinsAddedEvent> SummarizeSingleStackBets(ICamel camel, IEnumerable<IBetCard> betsStack)
        {
            List<ICoinsAddedEvent> rewards = new();
            var bets = BetCardsManager.CountCoins(camel, betsStack);
            foreach (var bet in bets)
            {
                ((Player)bet.Key).AddCoins(bet.Value);
                rewards.Add(new CoinsAddedEvent(bet.Key, bet.Value));
            }
            return rewards;
        }

        private void RefillDicer()
        {
            dicer.Reset();
            ActionEventsCollector.AddEvent(new DicerRefilledEvent());
        }

        private void RemoveAllAudienceTiles()
        {
            List<IAudienceTileRemovementEvent> audienceTileRemovementEvents = new();

            var fieldsWithAudienceTile = fields.Where(field => field.AudienceTile != null);
            foreach (Field field in fieldsWithAudienceTile)
            {
                audienceTileRemovementEvents.Add(new AudienceTileRemovementEvent(field.Index, field.AudienceTile));
                field.RemoveAudienceTile();
            }

            ActionEventsCollector.AddEvent(new AllAudienceTilesRemovementEvent(audienceTileRemovementEvents));
        }

        private void SetNextPlayer()
        {
            betManager.RegenerateGuid();
            cardManager.RegenerateGuid();
            tilesManager.RegenerateGuid();

            int currentIndex = players.ToList().IndexOf(currentPlayer);
            int playersCount = players.Count;
            ActionEventsCollector.AddEvent(new ChangedCurrentPlayerEvent(currentPlayer, currentPlayer = players.ElementAt(++currentIndex % playersCount)));
        }
    }
}
