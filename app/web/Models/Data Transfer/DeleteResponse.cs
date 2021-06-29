namespace SignalBox.Web.Dto
{
    public class DeleteResponse
    {
        public DeleteResponse(long id, string resouceUrl, bool success)
        {
            Id = id;
            ResouceUrl = resouceUrl;
            Success = success;
        }

        public long Id { get; set; }
        public string ResouceUrl { get; set; }
        public bool Success { get; set; }
    }
}