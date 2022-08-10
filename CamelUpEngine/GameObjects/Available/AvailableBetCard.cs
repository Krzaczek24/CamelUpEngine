using CamelUpEngine.Core.Enums;
using System;

namespace CamelUpEngine.GameObjects.Available
{
    public interface IAvailableBetCard : IColourable, IAvailable
    {

    }

    internal sealed class AvailableBetCard : IAvailableBetCard
    {
        public Colour Colour { get; }
        public Guid DrawGuid { get; }

        public AvailableBetCard(Colour colour, Guid drawGuid)
        {
            Colour = colour;
            DrawGuid = drawGuid;
        }

        public override string ToString() => $"{Colour} bet card";
    }
}
