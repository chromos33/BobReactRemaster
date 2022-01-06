using System;

namespace BobReactRemaster.Exceptions
{
    public class EmptyListExeption : Exception
    {
        public EmptyListExeption(string message) : base(message) { }
    }
}
