﻿#if DEBUG
using CamelUpEngine.Core.Actions;
using CamelUpEngine.GameTools;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.Helpers.TestHelpers
{
    public static class ActionHelper
    {
        public static T GetActionEvent<T>(this IEnumerable<IActionEvent> source) where T : class, IActionEvent => source.Single(step => step is T) as T;
        public static ActionEvents<T> GetActionEvents<T>(this IEnumerable<IActionEvent> source) where T : class, IActionEvent => new(source.Where(step => step is T).Cast<T>().ToList());
    }
}
#endif