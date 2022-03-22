using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;

namespace SignalBox.Web.Dto
{
    public class PaginateRequest : IPaginate
    {
        [FromQuery(Name = "page")]
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [FromQuery(Name = "pageSize")]
        [Range(1, 100)]
        public int? PageSize { get; set; }
    }
}