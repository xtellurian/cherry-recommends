using System;

namespace SignalBox.Core
{
    public class ModelInvokationException : SignalBoxException
    {
        public string ModelResponseContent { get; }

        public ModelInvokationException(ModelRegistration model, Exception inner, string modelResponse)
        : base($"Model ({model.Id}) with scoringUrl {model.ScoringUrl} returned an error.", inner)
        {
            this.ModelResponseContent = modelResponse;
        }
        public ModelInvokationException(string content) : base("Model responded with a error")
        {
            this.ModelResponseContent = content;
        }

    }
}