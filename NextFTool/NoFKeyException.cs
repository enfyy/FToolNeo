using System;

namespace NextFTool
{
    public class NoFKeyException : Exception
    {
        public NoFKeyException()
        {
        }

        public NoFKeyException(string message)
            : base(message)
        {
        }
    }
}

