namespace SignalBox.Core
{
    public class OfferStats
    {
        public OfferStats()
        { }

        public OfferStats(Offer offer, int numberAccepted, int numberRejected, string scoreName, double scoreValue)
        {
            Offer = offer;
            NumberAccepted = numberAccepted;
            NumberRejected = numberRejected;
            ScoreName = scoreName;
            ScoreValue = scoreValue;
        }

        // The scope is the set of properties that we segment the users by
        // for example, we will have "Country: UK" and "Country: France" as scopes
        public Offer Offer { get; set; }
        public int NumberAccepted { get; set; }
        public int NumberRejected { get; set; }
        public string ScoreName { get; set; }
        public double ScoreValue { get; set; }
    }
}