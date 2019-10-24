using System;

namespace NextFTool
{
    public class InvalidDelayException : Exception
    {
        public InvalidDelayException()
        {
        }

        public InvalidDelayException(string message)
            : base(message)
        {
        }
    }
}

