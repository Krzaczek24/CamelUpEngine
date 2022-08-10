using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Core.Actions.Events
{
    public interface IMadCamelColourSwitchedEvent : IActionEvent
    {
        public Colour From { get; }
        public Colour To { get; }
        public MadCamelColourSwitchReason SwitchReason { get; }
    }

    internal class MadCamelColourSwitchedEvent : IMadCamelColourSwitchedEvent
    {
        public Colour From { get; }
        public Colour To { get; }
        public MadCamelColourSwitchReason SwitchReason { get; }

        public MadCamelColourSwitchedEvent(Colour from, Colour to, MadCamelColourSwitchReason switchReason)
        {
            From = from;
            To = to;
            SwitchReason = switchReason;
        }
    }
}
