using System;

namespace SignalBox.Core
{
    public class RecommenderInvokationException : SignalBoxException
    {
        public RecommenderInvokationException(string title, string message) : base(title, message)
        { }
        public RecommenderInvokationException(string title, string message, System.Exception inner) : base(title, message, inner)
        { }
    }
}