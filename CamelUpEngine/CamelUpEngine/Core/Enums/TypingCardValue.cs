namespace CamelUpEngine.Core.Enums
{
    public enum TypingCardValue
    {
        Low = 2,
        Medium = 3,
        High = 5
    }

    public interface IValuable
    {
        public TypingCardValue Value { get; }
    }
}
