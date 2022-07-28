using CamelUpEngine.Core.Enums;
using CamelUpEngine.GameObjects;
using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface ICamelMovedEvent : IActionEvent
    {
        public IReadOnlyCollection<ICamel> Camels { get; }
        public int FromFieldIndex { get; }
        public int ToFieldIndex { get; }
        public StackPutType PutType { get; }
    }

    internal class CamelsMovedEvent : ICamelMovedEvent
    {
        public IReadOnlyCollection<ICamel> Camels { get; }
        public int FromFieldIndex { get; }
        public int ToFieldIndex { get; }
        public StackPutType PutType { get; }

        public CamelsMovedEvent(List<Camel> camels, int fromFieldIndex, int toFieldIndex, StackPutType putType = StackPutType.Top)
        {
            Camels = camels;
            FromFieldIndex = fromFieldIndex;
            ToFieldIndex = toFieldIndex;
            PutType = putType;
        }
    }
}
