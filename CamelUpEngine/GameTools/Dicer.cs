using CamelUpEngine.Core.Actions.Events;
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
        private readonly List<Dice> remainingDices = new();
        private readonly List<DrawnDice> drawnDices = new();
        public IReadOnlyCollection<IDrawnDice> DrawnDices => drawnDices.ToList();
        public bool IsEmpty => !remainingDices.Any();

        public Dicer()
        {
            Reset();
        }

        public IDrawnDice DrawDice()
        {
            Dice dice = remainingDices.OrderBy(_ => Guid.NewGuid()).FirstOrDefault() ?? throw new NoMoreDicesToDrawException();            
            DrawnDice drawnDice = new(dice);
            remainingDices.Remove(dice);
            drawnDices.Add(drawnDice);
            return drawnDice;
        }

        public void Reset()
        {
            drawnDices.Clear();
            remainingDices.Clear();
            remainingDices.AddRange(ColourHelper.AllDiceColours.Select(colour => new Dice(colour)));
        }

        public static IReadOnlyCollection<IDrawnDice> DrawDicesForInitialCamelsPlacement()
        {
            return ColourHelper.AllCamelColours.Select(colour => new Dice(colour).Draw()).ToList();
        }
    }
}
