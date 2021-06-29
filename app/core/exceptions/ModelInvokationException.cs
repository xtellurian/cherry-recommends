using System;

namespace SignalBox.Core
{
    public class ModelInvokationException : SignalBoxException
    {
        public ModelInvokationException(ModelRegistration model, Exception inner)
        : base($"Model ({model.Id}) with scoringUrl {model.ScoringUrl} returned an error.", inner)
        { }
        public ModelInvokationException(string message) : base(message)
        { }

    }
}