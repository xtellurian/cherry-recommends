namespace SignalBox.Core
{
#nullable enable
    // This is a public API
    public interface IRecommendationDto : IModelOutput
    {
        string? Trigger { get; }
        System.DateTimeOffset Created { get; }
        Business? Business { get; }
        string? BusinessId { get; }
        Customer? Customer { get; }
        string? CustomerId { get; }
    }
}