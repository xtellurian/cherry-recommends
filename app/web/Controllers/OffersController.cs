using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class OffersController : EntityControllerBase<Offer>
    {
        private readonly ILogger<OffersController> _logger;
        private readonly OfferWorkflows workflows;
        private readonly IExperimentStore experimentStore;

        public OffersController(ILogger<OffersController> logger,
                                OfferWorkflows workflows,
                                IExperimentStore experimentStore,
                                IOfferStore store) : base(store)
        {
            _logger = logger;
            this.workflows = workflows;
            this.experimentStore = experimentStore;
        }

        /// <summary>Creates a new offer.</summary>
        [HttpPost]
        public async Task<Offer> Create(CreateOfferDto dto)
        {
            return await workflows.CreateOffer(dto.Name, dto.Price, dto.Cost, dto.Currency, dto.DiscountCode);
        }
    }

}
