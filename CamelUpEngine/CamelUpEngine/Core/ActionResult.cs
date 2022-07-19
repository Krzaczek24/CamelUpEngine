namespace CamelUpEngine.Core
{
    public interface IActionResult
    {
        public bool Success { get; }
    }

    internal class ActionResult : IActionResult
    {
        public virtual bool Success { get; set; }
    }

    internal class ActionFailure : ActionResult
    {
        public override bool Success => false;
    }

    internal class ActionSuccess : ActionResult
    {
        public override bool Success => true;
    }
}
