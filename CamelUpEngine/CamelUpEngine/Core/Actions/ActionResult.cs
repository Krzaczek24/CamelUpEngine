using CamelUpEngine.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions
{
    public interface IActionResult
    {
        public IReadOnlyCollection<IActionStep> Actions { get; }
        public bool Success => Actions.Last() as CamelUpGameException == null;
    }

    internal class ActionResult : IActionResult
    {
        private List<IActionStep> actions = new();

        public IReadOnlyCollection<IActionStep> Actions => actions;
        
        public void AddActionStep(IActionStep action) => actions.Add(action);
    }
}
