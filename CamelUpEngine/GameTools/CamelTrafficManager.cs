using CamelUpEngine.Core.Actions;
using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.Extensions;
using CamelUpEngine.GameObjects;
using CamelUpEngine.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class CamelTrafficManager
    {
        private readonly List<Field> fields;
        private readonly Dictionary<Colour, Field> camelPositions;

        private IReadOnlyDictionary<Colour, int> camelPositionsCache = null;
        private IReadOnlyCollection<ICamel> allCamelsOrderCache = null;
        private IReadOnlyCollection<ICamel> camelsOrderCache = null;

        public bool AnyCamelPassFinishLine { get; private set; }
        public IReadOnlyCollection<IField> Fields => fields;
        public IReadOnlyDictionary<Colour, int> CamelPositions => camelPositionsCache ??= camelPositions.ToDictionary(entry => entry.Key, entry => entry.Value.Index);
        public IReadOnlyCollection<ICamel> OrderedAllCamels => allCamelsOrderCache ??= fields.Reverse<IField>().SelectMany(field => field.Camels).ToList();
        public IReadOnlyCollection<ICamel> OrderedCamels => camelsOrderCache ??= OrderedAllCamels.Where(camel => !camel.IsMad).ToList();

        internal CamelTrafficManager(IEnumerable<Field> fields)
        {
            this.fields = fields.ToList();
            camelPositions = GetCamelPositions();
        }

        public CamelTrafficManager(IEnumerable<IField> fields)
        {
            this.fields = fields.GetDeepCopy();
            camelPositions = GetCamelPositions();
        }

        public IReadOnlyCollection<IActionEvent> MoveCamel(Colour colour, int value)
        {
            List<IActionEvent> events = new();

            bool isMadColour = ColourHelper.IsMadColour(colour);
            if (isMadColour)
            {
                value = -value;
                if (ShouldSwitchMadColour(colour, out MadCamelColourSwitchReason madCamelColourSwitchReason))
                {
                    Colour originalColour = colour;
                    colour = ColourHelper.GetOppositeMadColour(colour);
                    events.Add(new MadCamelColourSwitchedEvent(originalColour, colour, madCamelColourSwitchReason));
                }
            }

            // camel move
            Field field = camelPositions[colour];
            List<Camel> camels = field.TakeOffCamel(colour);
            int newFieldIndex = field.Index + value;
            events.Add(new CamelsMovedEvent(camels, field.Index, newFieldIndex));
            if (DoesCamelGoThroughFinishLine(newFieldIndex))
            {
                PerformEndingCamelMove(camels, newFieldIndex);
                AnyCamelPassFinishLine = true;
                return events;
            }
            field = fields[newFieldIndex - 1];

            // additional move if camel ended move on audience tile
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                events.Add(new CamelsStoodOnAudienceTileEvent(audienceTile));
                events.Add(new CoinsAddedEvent(audienceTile.Owner, ((Player)audienceTile.Owner).AddCoins(1)));
                newFieldIndex = field.Index + (isMadColour ? -audienceTile.MoveValue : audienceTile.MoveValue);
                int prettyNewFieldIndex = ((newFieldIndex + fields.Count - 1) % fields.Count) + 1;
                events.Add(new CamelsMovedEvent(camels, field.Index, prettyNewFieldIndex, audienceTile.Side.ToStackPutType()));
                if (DoesCamelGoThroughFinishLine(newFieldIndex))
                {
                    PerformEndingCamelMove(camels, newFieldIndex);
                    AnyCamelPassFinishLine = true;

                    ClearCamelCaches();
                    return events;
                }
                field = fields[newFieldIndex - 1];
                if (audienceTile.Side == AudienceTileSide.Booing)
                {
                    field.PutCamels(camels, StackPutType.Bottom);
                    camels.ForEach(camel => camelPositions[camel.Colour] = field);

                    ClearCamelCaches();
                    return events;
                }
            }

            field.PutCamels(camels);
            camels.ForEach(camel => camelPositions[camel.Colour] = field);

            ClearCamelCaches();
            return events;
        }

        private void PerformEndingCamelMove(List<Camel> camels, int newFieldIndex)
        {
            Field field;
            if (newFieldIndex <= 0)
            {
                (field = fields.First()).PutCamels(camels, StackPutType.Bottom);
            }
            else
            {
                (field = fields.Last()).PutCamels(camels);
            }
            camels.ForEach(camel => camelPositions[camel.Colour] = field);
        }

        private bool DoesCamelGoThroughFinishLine(int newFieldIndex) => newFieldIndex <= 0 || newFieldIndex > fields.Count;

        private bool ShouldSwitchMadColour(Colour colour, out MadCamelColourSwitchReason madCamelColourSwitchReason)
        {
            if (AreMadCamelsOnTheSameField() && IsNearestCamelOnBackMad(colour))
            {
                madCamelColourSwitchReason = MadCamelColourSwitchReason.OtherMadCamelIsDirectlyOnBackOfOtherOne;
                return true;
            }

            if (!AnyNotMadCamelsOnBack(colour) && AnyNotMadCamelsOnBack(ColourHelper.GetOppositeMadColour(colour)))
            {
                madCamelColourSwitchReason = MadCamelColourSwitchReason.OnlyOneMadCamelIsCarryingNonMadCamels;
                return true;
            }

            madCamelColourSwitchReason = MadCamelColourSwitchReason.UNDEFINED;
            return false;
        }

        private bool IsNearestCamelOnBackMad(Colour colour) => camelPositions[colour].Camels.TakeUntil(camel => camel.Colour == colour).LastOrDefault()?.IsMad ?? false;
        private bool AnyNotMadCamelsOnBack(Colour colour) => camelPositions[colour].Camels.TakeUntil(camel => camel.Colour == colour).Where(camel => !camel.IsMad).Any();
        private bool AreMadCamelsOnTheSameField() => camelPositions[Colour.White] == camelPositions[Colour.Black];

        private Dictionary<Colour, Field> GetCamelPositions()
        {
            Dictionary<Colour, Field> camelPositions = new();

            foreach (var field in fields)
            {
                foreach (var camel in field.Camels)
                {
                    camelPositions.Add(camel.Colour, field);
                }
            }

            return camelPositions;
        }

        private void ClearCamelCaches()
        {
            camelPositionsCache = null;
            camelsOrderCache = null;
            allCamelsOrderCache = null;
        }
    }
}
