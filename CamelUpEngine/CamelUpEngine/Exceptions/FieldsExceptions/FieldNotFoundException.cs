namespace CamelUpEngine.Exceptions
{
    public class FieldNotFoundException : CamelUpGameException
    {
        public int FieldIndex { get; }

        public FieldNotFoundException(int fieldIndex) : base($"There is no field with index of {fieldIndex} on board")
        {
            FieldIndex = fieldIndex;
        }

        public FieldNotFoundException(int fieldIndex, string message) : base(message)
        {
            FieldIndex = fieldIndex;
        }
    }
}
