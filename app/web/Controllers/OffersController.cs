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
    public class OffersController : ControllerBase
    {
        private readonly ILogger<OffersController> _logger;
        private readonly OfferWorkflows workflows;
        private readonly IExperimentStore experimentStore;
        private readonly IOfferStore offerStore;

        public OffersController(ILogger<OffersController> logger,
                                OfferWorkflows workflows,
                                IExperimentStore experimentStore,
                                IOfferStore offerStore)
        {
            _logger = logger;
            this.workflows = workflows;
            this.experimentStore = experimentStore;
            this.offerStore = offerStore;
        }

        [HttpPost]
        public async Task<Offer> Create(CreateOfferDto dto)
        {
            return await workflows.CreateOffer(dto.Name, dto.Price, dto.Cost, dto.Currency, dto.DiscountCode);
        }

        [HttpGet]
        public async Task<IEnumerable<Offer>> List()
        {
            return await offerStore.List();
        }

        [HttpGet("{id}")]
        public async Task<Offer> Get(long id)
        {
            return await offerStore.Read(id);
        }

        // [HttpPost("experiments/{experimentId}/offers")]
        // public async Task<Offer> GetPersonalisedOffer(string experimentId, [FromBody] GetPersonalizedOffer model)
        // {
        //     var experiment = await experimentStore.Read(experimentId);
        //     var result = await scientist.GetPersonalizedOffer(
        //         new OfferingContext
        //         {
        //             TrackedUser = model.User,
        //             // TrackedUserProperties = model.Properties,
        //             Experiment = experiment
        //         }, offerStore);
        //     return result;
        // }
    }

}
