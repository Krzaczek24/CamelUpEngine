using System.Collections.Generic;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IAllAudienceTilesRemovementEvent : IActionSubEvents<IAudienceTileRemovementEvent>, IActionEvent
    {
        
    }

    internal class AllAudienceTilesRemovementEvent : ActionSubEvents<IAudienceTileRemovementEvent>, IAllAudienceTilesRemovementEvent
    {
        public AllAudienceTilesRemovementEvent(IEnumerable<IAudienceTileRemovementEvent> subEvents) : base(subEvents)
        {
            
        }
    }
}
