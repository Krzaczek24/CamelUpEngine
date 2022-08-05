using CamelUpEngine.Core.Actions;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers
{
    public static class ActionHelper
    {
        public static T GetEvent<T>(this IEnumerable<IActionEvent> source) where T : class, IActionEvent => source.SingleOrDefault(step => step is T) as T;
        public static IEnumerable<T> GetEvents<T>(this IEnumerable<IActionEvent> source) where T : class, IActionEvent => source.Where(step => step is T).Cast<T>().ToList();
    }
}