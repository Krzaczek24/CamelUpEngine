using CamelUpEngine.Core.Actions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers
{
    public static class ActionHelper
    {
        public static T GetActionStep<T>(this IActionResult source) where T : class, IActionStep => source.Steps.GetStep<T>();
        public static T GetStep<T>(this IEnumerable<IActionStep> source) where T : class, IActionStep => source.Single(step => step is T) as T;

        public static IReadOnlyCollection<T> GetActionSteps<T>(this IActionResult source) where T : class, IActionStep => source.Steps.GetSteps<T>();
        public static IReadOnlyCollection<T> GetSteps<T>(this IEnumerable<IActionStep> source) where T : class, IActionStep => source.Where(step => step is T).Cast<T>().ToList();
    }
}
