using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

#nullable enable
namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class ExperimentsController : ControllerBase
    {
        private readonly ILogger<ExperimentsController> logger;
        private readonly ExperimentWorkflows experimentWorkflows;
        private readonly PresentationsWorkflows presentationsWorkflows;
        private readonly IExperimentStore experimentStore;

        public ExperimentsController(
            ILogger<ExperimentsController> logger,
            IExperimentStore experimentStore,
            ExperimentWorkflows experimentWorkflows,
            PresentationsWorkflows presentationsWorkflows)
        {
            this.logger = logger;
            this.experimentStore = experimentStore;
            this.experimentWorkflows = experimentWorkflows;
            this.presentationsWorkflows = presentationsWorkflows;
        }



        [HttpPost]
        public async Task<CreateExperimentResultDto> CreateExperiment([FromBody] CreateExperimentDto dto)
        {
            var experiment = await experimentWorkflows.CreateExperiment(dto.Name, dto.OfferIds, dto.ConcurrentOffers);
            return new CreateExperimentResultDto(experiment);
        }

        [HttpGet]
        public async Task<IEnumerable<Experiment>> GetExperimentList()
        {
            return await experimentStore.List();
        }

        [HttpGet("{id}")]
        public async Task<Experiment> GetExperiment(long id)
        {
            return await experimentStore.Read(id);
        }

        [HttpGet("{id}/offers")]
        public async Task<IEnumerable<Offer>> GetExperimentOffers(long id)
        {
            var experiment = await experimentStore.Read(id);
            return experiment.Offers;
        }

        [HttpGet("{id}/results")]
        public async Task<ExperimentResults> GetExperimentResults(long id, string? scorer = null)
        {
            return await experimentWorkflows.CalculateResults(id, scorer);
        }

        [HttpPost("{id}/presentation")]
        [HttpPost("{id}/recommendation")]
        public async Task<OfferRecommendation> PresentExperimentOffers(long id, [FromBody] RecommendationContextDto dto)
        {
            return await presentationsWorkflows.RecommendOffer(id, dto.CommonUserId, dto.Features);
        }

        [HttpPost("{id}/outcome")]
        public async Task<PresentationOutcome> TrackResults(long id, [FromBody] TrackPresentationDto dto)
        {
            return await presentationsWorkflows.TrackPresentationOutcome(id,
                                                                         dto.IterationId,
                                                                         dto.RecommendationId,
                                                                         dto.OfferId,
                                                                         dto.Outcome);
        }
    }
}
