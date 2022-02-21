using Microsoft.AspNetCore.Mvc;

namespace SignalBox.Web.Dto
{
    public class SearchEntities
    {
        [FromQuery(Name = "term")]
        public string Term { get; set; }
        [FromQuery(Name = "scope")]
        public string Scope { get; set; }
    }
}