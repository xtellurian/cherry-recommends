namespace SignalBox.Core
{
    /// <summary>
    /// Thrown when a common Id is invalid.
    /// </summary>
    public class CommonIdException : BadRequestException
    {
        public CommonIdException(string commonId, string message) : base("Invalid Common Id", message)
        {
            this._status = 400;
            this.CommonId = commonId;
        }

        public string CommonId { get; }
    }
}