namespace SignalBox.Core
{
    public class EvaluationResult
    {
        public EvaluationResult(string result)
        {
            Result = result;
        }

        public string Result { get; set; }
    }
}