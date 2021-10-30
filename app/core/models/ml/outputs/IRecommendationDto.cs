namespace SignalBox.Core
{
    // This is a public API
    public interface IRecommendationDto : IModelOutput
    {
        TrackedUser Customer { get; }
    }
}