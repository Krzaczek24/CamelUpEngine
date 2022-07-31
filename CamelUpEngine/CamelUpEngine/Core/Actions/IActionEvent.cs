using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CamelUpEngine.Core.Actions
{
    public interface IActionEvent { }
    public interface ISubEvents : IActionEvent { }
    public interface ISubEvents<T> where T : IActionEvent
    {
        public IReadOnlyCollection<T> SubEvents { get; }
    }

    public class ActionEvents<T> : ReadOnlyCollection<T> where T : IActionEvent
    {
        internal ActionEvents() : base(new List<T>()) { }
        internal ActionEvents(IList<T> list) : base(list) { }
    }

    public class ActionEvents : ActionEvents<IActionEvent>
    {
        internal ActionEvents() : base() { }
        internal ActionEvents(IList<IActionEvent> list) : base(list) { }
    }
}
