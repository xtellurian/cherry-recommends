namespace SignalBox.Web.Dto
{
    public class CreateApiKeyResponseDto
    {
        public CreateApiKeyResponseDto(string name, string apiKey)
        {
            ApiKey = apiKey;
        }

        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}