using CamelUpEngine.Core.Enums;
using CamelUpEngine.Exceptions;
using CamelUpEngine.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameObjects
{
    public interface IField
    {
        public int Index { get; }
        public IAudienceTile AudienceTile { get; }
        public IReadOnlyCollection<ICamel> Camels { get; }
    }

    internal sealed class Field : IField
    {
        private readonly List<Camel> camels = new();

        public int Index { get; }
        public IAudienceTile AudienceTile { get; private set; }
        public IReadOnlyCollection<ICamel> Camels => camels;

        private Field() { }
        public Field(int number)
        {
            Index = number;
        }

        private bool HasCamel(Colour colour) => Camels.Any(camel => camel.Colour == colour);

        public List<Camel> TakeOffCamel(Colour colour)
        {
            if (!HasCamel(colour))
            {
                throw new NoCamelOnFieldFoundException(colour, Index);
            }

            var takenCamels = camels.TakeUntil(camel => camel.Colour == colour, true).ToList();
            takenCamels.ForEach(camel => camels.Remove(camel));
            return takenCamels;
        }

        public void PutCamels(List<Camel> camels, StackPutType putType = StackPutType.Top)
        {
            switch (putType)
            {
                case StackPutType.Top: this.camels.InsertRange(0, camels); return;
                case StackPutType.Bottom: this.camels.AddRange(camels); return;
            }
        }

        public void RemoveAudienceTile()
        {
            if (AudienceTile == null)
            {
                throw new NoAudienceTileOnFieldFoundException(Index);
            }

            AudienceTile = null;
        }

        public void PutAudienceTile(AudienceTile audienceTile)
        {
            if (Index == 1)
            {
                throw new PuttingAudienceTileOnStartFieldException(Index);
            }
            else if (AudienceTile != null)
            {
                throw new FieldAlreadyOccupiedByAudienceTileExcception(Index);
            }
            else if (Camels.Any())
            {
                throw new FieldOccupiedByCamelException(Index);
            }

            AudienceTile = audienceTile;
        }

        public override string ToString() => $"{Index}. field {CamelsDesc()} and {AudienceTileDesc()}";
        private string CamelsDesc() => camels.Any() ? $"with {camels.Count} camel{(camels.Count > 1 ? "s" : "")}" : "with no camels";
        private string AudienceTileDesc() => $"with{(AudienceTile == null ? "out" : "")} audience tile";
    }

    internal static class FieldHelper
    {
        internal static List<Field> GetDeepCopy(this IEnumerable<IField> fields)
        {
            List<Field> copiedFields = new();

            foreach (var field in fields)
            {
                Field copiedField = new(field.Index);
                if (field.AudienceTile != null)
                {
                    copiedField.PutAudienceTile(new AudienceTile(field.AudienceTile.Owner, field.AudienceTile.Side));
                }
                copiedField.PutCamels(field.Camels.Select(camel => new Camel(camel.Colour)).ToList());
                copiedFields.Add(copiedField);
            }

            return copiedFields;
        }
    }
}
