using CamelUpEngine.Core.Actions;

namespace CamelUpEngine.GameTools
{
    internal class ActionCollector
    {
        private static ActionResult actionResult = new();

        public static void AddAction(IActionStep action) => actionResult.AddActionStep(action);

        public static IActionResult GetActions()
        {
            IActionResult result = actionResult;
            actionResult = new();
            return result;
        }
    }
}
