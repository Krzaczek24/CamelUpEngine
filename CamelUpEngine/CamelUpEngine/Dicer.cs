using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine
{
    public sealed class Dicer
    {
        private List<Dice> remainingDices = new List<Dice>();
        private List<DrawnDice> drawnDices = new List<DrawnDice>();
        public IReadOnlyCollection<DrawnDice> DrawnDices => drawnDices.AsReadOnly();

        public Dicer()
        {
            Reset();
        }

        public DrawnDice DrawDice()
        {
            Dice dice = remainingDices.OrderBy(d => Guid.NewGuid()).FirstOrDefault();
            if (dice == null)
            {
                return null;
            }
            
            DrawnDice drawnDice = new DrawnDice(dice);
            remainingDices.Remove(dice);
            drawnDices.Add(drawnDice);
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

        public static IReadOnlyCollection<DrawnDice> DrawDicesForInitialCamelsPlacement()
        {
            return ColourHelper.AllCamelColours.Select(colour => new DrawnDice(new Dice(colour))).ToList();
        }
    }
}
