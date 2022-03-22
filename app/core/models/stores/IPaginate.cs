namespace SignalBox.Core
{
#nullable enable
    public interface IPaginate
    {
        int Page { get; }

        int? PageSize { get; }

        /// <summary>
        /// Returns the maximum of Page and 1
        /// </summary>
        int SafePage => Page >= 1 ? Page : 1;
    }
}