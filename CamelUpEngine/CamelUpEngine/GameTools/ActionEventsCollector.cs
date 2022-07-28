using CamelUpEngine.Core.Actions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CamelUpEngine.GameTools
{
    public class ActionEvents<T> : ReadOnlyCollection<T> where T : IActionEvent
    {
        internal ActionEvents(IList<T> list) : base(list) { }
    }

    public class ActionEvents : ActionEvents<IActionEvent>
    {
        internal ActionEvents(IList<IActionEvent> list) : base(list) { }
    }

    internal static class ActionEventsCollector
    {
        private static Dictionary<Guid, IActionEvent> confirmationGuids = new();
        private static List<IActionEvent> events = new();

        public static void AddEvent(IActionEvent actionEvent) => events.Add(actionEvent);
        public static Guid AddUnconfirmedEvent(IActionEvent actionEvent)
        {
            Guid guid = Guid.NewGuid();
            events.Add(actionEvent);
            confirmationGuids.Add(guid, actionEvent);
            return guid;
        }
        public static bool ConfirmEvent(Guid guid) => confirmationGuids.Remove(guid);

        public static ActionEvents GetEvents()
        {
            var eventsCopy = events.Except(confirmationGuids.Values).ToList();
            events = new();
            confirmationGuids = new();
            return new(eventsCopy);
        }
    }
}
