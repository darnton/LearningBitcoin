using System;

namespace Bitcoin
{
    public class ParsingException : Exception
    {
        public ParsingException() : base() { }
        public ParsingException(string message) : base(message) { }
    }
}
