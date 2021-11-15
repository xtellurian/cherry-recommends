namespace SignalBox.Web.Dto
{
    public class StatusDto : DtoBase
    {
        public StatusDto(string status)
        {
            this.Status = status;
        }

        public string Status { get; set; }
    }
}