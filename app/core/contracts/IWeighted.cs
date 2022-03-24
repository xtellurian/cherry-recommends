namespace SignalBox.Core
{
    /// <summary>
    /// A collection of IWeighted can be normalised.
    /// </summary>
    public interface IWeighted
    {
        long Id { get; }
        double Weight { get; set; }
    }

}