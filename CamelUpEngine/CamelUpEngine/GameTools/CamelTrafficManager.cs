﻿using CamelUpEngine.Core.Actions.Events;
using CamelUpEngine.Core.Enums;
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
        public IReadOnlyDictionary<Colour, int> CamelPositions => camelPositionsCache ??= camelPositions.ToDictionary(entry => entry.Key, entry => entry.Value.Index);
        public IReadOnlyCollection<ICamel> AllCamelsOrder => allCamelsOrderCache ??= fields.Reverse<IField>().SelectMany(field => field.Camels).ToList();
        public IReadOnlyCollection<ICamel> CamelsOrder => camelsOrderCache ??= AllCamelsOrder.Where(camel => !camel.IsMad).ToList();

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

        public void MoveCamel(Colour colour, int value)
        {
            bool isMadColour = ColourHelper.IsMadColour(colour);
            if (isMadColour && ShouldSwitchMadColour(colour))
            {
                colour = ColourHelper.GetOppositeMadColour(colour);
            }

            // camel move
            Field field = camelPositions[colour];
            List<Camel> camels = field.TakeOffCamel(colour);
            int newFieldIndex = field.Index + value;
            ActionEventsCollector.AddEvent(new CamelsMovedEvent(camels, field.Index, newFieldIndex));
            if (DoesCamelGoThroughFinishLine(newFieldIndex))
            {
                PerformEndingCamelMove(camels, newFieldIndex);
                AnyCamelPassFinishLine = true;
                return;
            }
            field = fields[newFieldIndex - 1];

            // additional move if camel ended move on audience tile
            AudienceTile audienceTile = (AudienceTile)field.AudienceTile;
            if (audienceTile != null)
            {
                ActionEventsCollector.AddEvent(new CamelsStoodOnAudienceTileEvent(audienceTile));
                ActionEventsCollector.AddEvent(new CoinsAddedEvent(audienceTile.Owner, ((Player)audienceTile.Owner).AddCoins(1)));
                newFieldIndex = field.Index + audienceTile.MoveValue;
                ActionEventsCollector.AddEvent(new CamelsMovedEvent(camels, field.Index, newFieldIndex, audienceTile.Side.ToStackPutType()));
                if (DoesCamelGoThroughFinishLine(newFieldIndex))
                {
                    PerformEndingCamelMove(camels, newFieldIndex);
                    AnyCamelPassFinishLine = true;
                    return;
                }
                field = fields[newFieldIndex - 1];
                if (audienceTile.Side == AudienceTileSide.Booing)
                {
                    field.PutCamels(camels, StackPutType.Bottom);
                    camels.ForEach(camel => camelPositions[camel.Colour] = field);
                    return;
                }
            }

            field.PutCamels(camels);
            camels.ForEach(camel => camelPositions[camel.Colour] = field);

            ClearCamelCaches();
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

        private bool ShouldSwitchMadColour(Colour colour)
        {
            if (AreMadCamelsOnTheSameField() && IsNearestCamelOnBackMad(colour))
            {
                ActionEventsCollector.AddEvent(new MadCamelColourSwitchedEvent(MadCamelColourSwitchReason.OtherMadCamelIsDirectlyOnBackOfOtherOne));
                return true;
            }

            if (!AnyNotMadCamelsOnBack(colour) && AnyNotMadCamelsOnBack(ColourHelper.GetOppositeMadColour(colour)))
            {
                ActionEventsCollector.AddEvent(new MadCamelColourSwitchedEvent(MadCamelColourSwitchReason.OnlyOneMadCamelIsCarryingNonMadCamels));
                return true;
            }

            return false;
        }

        private bool IsNearestCamelOnBackMad(Colour colour) => camelPositions[colour].Camels.TakeWhile(camel => camel.Colour == colour).LastOrDefault()?.IsMad ?? false;
        private bool AnyNotMadCamelsOnBack(Colour colour) => camelPositions[colour].Camels.TakeWhile(camel => camel.Colour == colour).Where(camel => !camel.IsMad).Any();
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