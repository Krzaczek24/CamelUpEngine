using CamelUpEngine.Core.Actions.ActionSteps;
using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public sealed class Dicer
    {
        public const int DICE_DRAW_REWARD = 1;

        private IList<Dice> remainingDices = new List<Dice>();
        private IList<DrawnDice> drawnDices = new List<DrawnDice>();
        public IReadOnlyCollection<IDrawnDice> DrawnDices => drawnDices.ToList();

        public Dicer()
        {
            Reset();
        }

        public IDrawnDice DrawDice()
        {
            Dice dice = remainingDices.OrderBy(d => Guid.NewGuid()).FirstOrDefault();
            if (dice == null)
            {
                return null;
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
