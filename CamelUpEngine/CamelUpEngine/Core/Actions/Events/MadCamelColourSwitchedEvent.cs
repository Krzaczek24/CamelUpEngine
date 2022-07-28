using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IMadCamelColourSwitchedEvent : IActionEvent
    {
        public MadCamelColourSwitchReason SwitchReason { get; }
    }

    internal class MadCamelColourSwitchedEvent : IMadCamelColourSwitchedEvent
    {
        public MadCamelColourSwitchReason SwitchReason { get; }

        public MadCamelColourSwitchedEvent(MadCamelColourSwitchReason switchReason)
        {
            SwitchReason = switchReason;
        }
    }
}
