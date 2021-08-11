using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/RewardSelectors")]
    public class RewardSelectorsController : EntityControllerBase<RewardSelector>
    {
        private readonly ILogger<ActionsController> logger;

        public RewardSelectorsController(ILogger<ActionsController> logger,
                                IRewardSelectorStore actionNameSelectorStore) : base(actionNameSelectorStore)
        {
            this.logger = logger;
        }

        [HttpPost]
        public async Task<RewardSelector> Create(CreateRewardSelectorDto dto)
        {
            if (Enum.TryParse<SelectorTypes>(dto.SelectorType, out var selectorType))
            {
                var selector = await store.Create(new RewardSelector(dto.ActionName, selectorType, dto.Category));
                await store.Context.SaveChanges();
                return selector;
            }
            else
            {
                throw new BadRequestException($"Couldn't parse selector type: {dto.SelectorType}");
            }
        }
    }
}
