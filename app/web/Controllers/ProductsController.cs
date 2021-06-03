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
    public class ProductsController : EntityControllerBase<Product>
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
            return await workflows.CreateProduct(dto.Name, dto.ProductId, dto.Description);
        }

        /// <summary>Adds or updates skus for the given product Id.</summary>
        [HttpPut("{productId}/skus")]
        public async Task<Product> SetSkus(string productId, IEnumerable<CreateOrUpdateSkuDto> skus)
        {
            return await workflows.UpdateProductSkusFromProductId(productId, 
                skus.Select(_ => new ProductWorkflows.SkuInput(_.Name, _.SkuId, _.Description, _.Price)));
        }
    }
}