using CamelUpEngine.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    internal class ActionEventsCollector
    {
        private static Dictionary<Guid, IActionEvent> confirmationGuids = new();
        private static List<IActionEvent> gameEvents = new();
        private static List<IActionEvent> actionEvents = new();

        public static void AddEvent(IActionEvent actionEvent) => actionEvents.Add(actionEvent);
        public static void AddEvent(IEnumerable<IActionEvent> actionEvent) => actionEvents.AddRange(actionEvent);
        public static Guid AddUnconfirmedEvent(IActionEvent actionEvent)
        {
            Guid guid = Guid.NewGuid();
            actionEvents.Add(actionEvent);
            confirmationGuids.Add(guid, actionEvent);
            return guid;
        }
        public static bool ConfirmEvent(Guid guid) => confirmationGuids.Remove(guid);

        public static ActionEvents GetGameEvents() => new(gameEvents.Concat(actionEvents.Except(confirmationGuids.Values)).ToList());
        public static ActionEvents GetActionEvents()
        {
            var eventsCopy = actionEvents.Except(confirmationGuids.Values).ToList();
            gameEvents.AddRange(actionEvents);
            actionEvents = new();
            confirmationGuids = new();
            return new(eventsCopy);
        }

        public static void Reset()
        {
            gameEvents = new();
            actionEvents = new();
            confirmationGuids = new();
        }
    }
}
