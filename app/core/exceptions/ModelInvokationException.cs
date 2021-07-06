using System;

namespace SignalBox.Core
{
    public class ModelInvokationException : SignalBoxException
    {
        public string ModelResponseContent { get; }

        public ModelInvokationException(ModelRegistration model, Exception inner)
        : base($"Model ({model.Id}) with scoringUrl {model.ScoringUrl} returned an error.", inner)
        { }
        public ModelInvokationException(string content) : base("Model responded with a error")
        {
            this.ModelResponseContent = content;
        }

    }
}