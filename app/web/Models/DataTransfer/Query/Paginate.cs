using Microsoft.AspNetCore.Mvc;

namespace SignalBox.Web.Dto
{
    public class PaginateRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        [FromQuery(Name = "pageSize")]
        public int? PageSize { get; set; }
    }
}