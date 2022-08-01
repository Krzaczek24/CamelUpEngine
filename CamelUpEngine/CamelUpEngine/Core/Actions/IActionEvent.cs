using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CamelUpEngine.Core.Actions
{
    public interface IActionEvent
    {

    }

    public class ActionEvents : ReadOnlyCollection<IActionEvent>
    {
        internal ActionEvents() : base(new List<IActionEvent>()) { }
        internal ActionEvents(IList<IActionEvent> list) : base(list) { }
    }
}
