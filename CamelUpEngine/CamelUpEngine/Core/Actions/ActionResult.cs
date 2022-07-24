using CamelUpEngine.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Core.Actions
{
    public interface IActionResult
    {
        public IReadOnlyCollection<IActionStep> Steps { get; }
        public bool Success => !(Steps.Last() is CamelUpGameException);
    }

    internal class ActionResult : IActionResult
    {
        private List<IActionStep> steps = new();

        public IReadOnlyCollection<IActionStep> Steps => steps;
        
        public void AddActionStep(IActionStep action) => steps.Add(action);
    }
}
