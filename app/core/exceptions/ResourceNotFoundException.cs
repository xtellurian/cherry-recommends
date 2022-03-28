using System;

namespace SignalBox.Core
{
    public class ResourceNotFoundException : SignalBoxException
    {
        public ResourceNotFoundException() : base("Resource Not Found")
        { }
        public ResourceNotFoundException(Exception inner) : base("Resource Not Found", inner)
        { }

        public override int Status => 404;
    }
}