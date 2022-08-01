using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions
{
    public interface IActionSubEvents : IActionEvent
    {
        public int Count { get; }
    }

    public interface IActionSubEvents<T> : IActionSubEvents where T : IActionEvent
    {
        public IReadOnlyCollection<T> SubEvents { get; }
    }

    internal abstract class ActionSubEvents<T> : IActionSubEvents<T> where T : IActionEvent
    {
        public IReadOnlyCollection<T> SubEvents { get; }

        public int Count => SubEvents.Count;

        public ActionSubEvents(IEnumerable<T> subEvents)
        {
            SubEvents = subEvents.ToList();
        }
    }
}
