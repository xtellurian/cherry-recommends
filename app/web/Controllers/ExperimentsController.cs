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
    public class ExperimentsController : EntityControllerBase<Experiment>
    {
        private readonly ILogger<ExperimentsController> logger;
        private readonly ExperimentWorkflows experimentWorkflows;
        private readonly PresentationsWorkflows presentationsWorkflows;

        public ExperimentsController(
            ILogger<ExperimentsController> logger,
            IExperimentStore experimentStore,
            ExperimentWorkflows experimentWorkflows,
            PresentationsWorkflows presentationsWorkflows) : base(experimentStore)
        {
            this.logger = logger;
            this.experimentWorkflows = experimentWorkflows;
            this.presentationsWorkflows = presentationsWorkflows;
        }

        /// <summary>Creates a new experiment resource.</summary>
        [HttpPost]
        public async Task<CreateExperimentResultDto> CreateExperiment([FromBody] CreateExperimentDto dto)
        {
            var experiment = await experimentWorkflows.CreateExperiment(dto.Name, dto.OfferIds, dto.ConcurrentOffers);
            return new CreateExperimentResultDto(experiment);
        }

        /// <summary>Returns the list of offers connected to the experiment.</summary>
        [HttpGet("{id}/offers")]
        public async Task<IEnumerable<Offer>> GetExperimentOffers(long id)
        {
            var experiment = await store.Read(id);
            return experiment.Offers;
        }

        /// <summary>Returns the results of the experiment.</summary>
        [HttpGet("{id}/results")]
        public async Task<ExperimentResults> GetExperimentResults(long id, string? scorer = null)
        {
            return await experimentWorkflows.CalculateResults(id, scorer);
        }

        /// <summary>Presents the optimum offer for a given user with a set of features.</summary>
        [HttpPost("{id}/presentation")]
        [HttpPost("{id}/recommendation")]
        public async Task<OfferRecommendation> PresentExperimentOffers(long id, [FromBody] RecommendationContextDto dto)
        {
            return await presentationsWorkflows.RecommendOffer(id, dto.CommonUserId, dto.Features);
        }

        /// <summary>Track the outcome of an offer that has been presented..</summary>
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
