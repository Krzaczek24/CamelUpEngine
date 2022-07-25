using CamelUpEngine.Core.Actions.Steps;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class Dicer
    {
        public const int DICE_DRAW_REWARD = 1;

        private readonly List<Dice> remainingDices = new();
        private readonly List<DrawnDice> drawnDices = new();
        public IReadOnlyCollection<IDrawnDice> DrawnDices => drawnDices.ToList();

        public Dicer()
        {
            Reset();
        }

        public IDrawnDice DrawDice()
        {
            Dice dice = remainingDices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
            if (dice == null)
            {
                throw new NoMoreDicesToDrawException();
            }
            
            DrawnDice drawnDice = new(dice);
            remainingDices.Remove(dice);
            drawnDices.Add(drawnDice);
            ActionCollector.AddAction(new DiceDrawnStep(drawnDice));
            return drawnDice;
        }

        public void Reset()
        {
            remainingDices.Clear();
            drawnDices.Clear();
            foreach (Colour colour in ColourHelper.AllDiceColours)
            {
                remainingDices.Add(new Dice(colour));
            }
        }

        public static IReadOnlyCollection<IDrawnDice> DrawDicesForInitialCamelsPlacement()
        {
            return ColourHelper.AllCamelColours.Select(colour => new DrawnDice(new Dice(colour))).ToList();
        }
    }
}
