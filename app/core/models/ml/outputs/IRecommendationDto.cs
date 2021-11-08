namespace SignalBox.Core
{
#nullable enable
    // This is a public API
    public interface IRecommendationDto : IModelOutput
    {
        string? Trigger { get; }
        System.DateTimeOffset Created { get; }
        TrackedUser Customer { get; }
    }
}