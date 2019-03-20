using System;

namespace Bitcoin
{
    public class ValidationException : Exception
    {
        public ValidationException() : base() { }
        public ValidationException(string message) : base(message) { }
    }
}
