using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ProductsController : CommonEntityControllerBase<Product>
    {
        private readonly ProductWorkflows workflows;

        public ProductsController(IProductStore store, ProductWorkflows workflows) : base(store)
        {
            this.workflows = workflows;
        }

        /// <summary>Creates a new product.</summary>
        [HttpPost]
        public async Task<Product> Create(CreateProductDto dto)
        {
            return await workflows.CreateProduct(dto.CommonId, dto.Name, dto.ListPrice, dto.Description, dto.DirectCost);
        }
    }
}