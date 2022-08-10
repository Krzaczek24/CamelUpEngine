using System;

namespace CamelUpEngine.Exceptions
{
    public class CamelUpGameException : Exception
    {
        public CamelUpGameException() : base() { }
        public CamelUpGameException(string message) : base(message) { }
    }
}
