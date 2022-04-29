namespace SignalBox.Web.Dto
{
    public class UpdateEmailChannelTriggerDto : DtoBase
    {
        /// <summary>
        /// Id of the List that triggers the Email flow
        /// </summary>
        public string ListId { get; set; }

        /// <summary>
        /// Name of the List that triggers the Email flow
        /// </summary>
        public string ListName { get; set; }
    }
}