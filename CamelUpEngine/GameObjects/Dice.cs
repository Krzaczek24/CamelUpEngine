using CamelUpEngine.Core.Enums;
using CamelUpEngine.Helpers;
using System;
using System.Linq;

namespace CamelUpEngine.GameObjects
{
    public interface IDice : IColourable { }

    public interface IDrawnDice : IDice
    {
        public int Value { get; }
    }

    internal sealed class Dice : IDice
    {
        public Colour Colour { get; }

        private Dice() { }
        public Dice(Colour colour)
        {
            Colour = colour;
        }

        public DrawnDice Draw() => new(this);

        public override string ToString() => $"{Colour} dice";
    }

    internal sealed class DrawnDice : IDrawnDice
    {
        public Colour Colour { get; }
        public int Value { get; }

        private DrawnDice() { }
        public DrawnDice(Dice dice)
        {
            Colour = ConvertColour(dice.Colour);
            Value = new Random().Next(3) + 1;         
        }

        private Colour ConvertColour(Colour colour)
        {
            if (colour == Colour.Mad)
            {
                return ColourHelper.MadColours.OrderBy(_ => Guid.NewGuid()).First();
            }

            return colour;
        }

        public override string ToString() => $"{Colour} dice with value of {Value}";
    }
}
