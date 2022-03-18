using System.Collections.Generic;
using SignalBox.Core.Recommenders;

namespace SignalBox.Core
{
    public class Audience : Entity
    {
        public long RecommenderId { get; set; }

        public RecommenderEntityBase Recommender { get; set; }
        public ICollection<Segment> Segments { get; set; }

        protected Audience()
        { }

        public Audience(RecommenderEntityBase recommender, ICollection<Segment> segments)
        {
            this.RecommenderId = recommender.Id;
            this.Recommender = recommender;
            this.Segments = segments;
        }
    }
}