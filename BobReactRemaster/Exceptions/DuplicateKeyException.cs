using System;

namespace BobReactRemaster.Exceptions
{
    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string message) : base(message){}
    }
}
