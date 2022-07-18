using CamelUpEngine.Exceptions.CamelsExceptions;
using CamelUpEngine.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine
{
    public interface IField
    {
        public int Index { get; }

        public IReadOnlyCollection<ICamel> GetCamels();
        //public bool HasCamel(Colour colour);
    }

    public sealed class Field : IField
    {
        private List<Camel> Camels { get; } = new List<Camel>();

        public int Index { get; }

        private Field() { }
        public Field(int number)
        {
            Index = number;
        }

        public ICollection<Camel> TakeOffCamel(Colour colour)
        {
            if (!Camels.Any(c => c.Colour == colour))
            {
                throw new NoCamelOnFieldFoundException(colour, Index);
            }

            var takenCamels = Camels.TakeUntil(camel => camel.Colour == colour, true).ToList();
            takenCamels.ForEach(camel => Camels.Remove(camel));
            return takenCamels;
        }

        public IReadOnlyCollection<ICamel> GetCamels() => Camels;

        public void PutCamelsOnTop(Camel camel) => PutCamelsOnTop(new[] { camel });
        public void PutCamelsOnTop(ICollection<Camel> camels) => Camels.InsertRange(0, camels);

        public void PutCamelsOnBottom(ICollection<Camel> camels) => Camels.AddRange(camels);

        //public bool HasCamel(Colour colour) => Camels.Any(c => c.Colour == colour);
    }
}
