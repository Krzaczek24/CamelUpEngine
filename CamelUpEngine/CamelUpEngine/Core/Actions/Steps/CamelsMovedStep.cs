using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface ICamelMovedStep : IActionStep
    {
        public IReadOnlyCollection<ICamel> Camels { get; }
        public int FromFieldIndex { get; }
        public int ToFieldIndex { get; }
        public StackPutType PutType { get; }
    }

    internal class CamelsMovedStep : ICamelMovedStep
    {
        public IReadOnlyCollection<ICamel> Camels { get; }
        public int FromFieldIndex { get; }
        public int ToFieldIndex { get; }
        public StackPutType PutType { get; }

        public CamelsMovedStep(List<Camel> camels, int fromFieldIndex, int toFieldIndex, StackPutType putType = StackPutType.Top)
        {
            Camels = camels;
            FromFieldIndex = fromFieldIndex;
            ToFieldIndex = toFieldIndex;
            PutType = putType;
        }
    }
}
