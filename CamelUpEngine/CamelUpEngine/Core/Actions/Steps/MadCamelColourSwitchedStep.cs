using CamelUpEngine.Core.Enums;

namespace CamelUpEngine.Core.Actions.Steps
{
    public interface IMadCamelColourSwitchedStep : IActionStep
    {
        public MadCamelColourSwitchReason SwitchReason { get; }
    }

    internal class MadCamelColourSwitchedStep : IMadCamelColourSwitchedStep
    {
        public MadCamelColourSwitchReason SwitchReason { get; }

        public MadCamelColourSwitchedStep(MadCamelColourSwitchReason switchReason)
        {
            SwitchReason = switchReason;
        }
    }
}
