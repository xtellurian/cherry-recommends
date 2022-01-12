using System;

namespace SignalBox.Core
{
    public class ResourceNotFoundException : SignalBoxException
    {
        public ResourceNotFoundException() : base("Resource Not Found")
        { }

        public override int Status => 404;
    }
}