using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SignalBox.Web.Dto
{
    public class BatchCreateOrUpdateCustomersDto : DtoBase
    {
        public IEnumerable<CreateOrUpdateCustomerDto> Items() => Customers ?? Users ?? Enumerable.Empty<CreateOrUpdateCustomerDto>();
        public IEnumerable<CreateOrUpdateCustomerDto> Users { get; set; }
        public IEnumerable<CreateOrUpdateCustomerDto> Customers { get; set; }
    }
}