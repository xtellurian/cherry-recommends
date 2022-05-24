using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SignalBox.Core.Workflows
{
    public class DiscountCodeWorkflows : IDiscountCodeWorkflow, IWorkflow
    {
        private readonly ILogger<DiscountCodeWorkflows> logger;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly IDiscountCodeStore discountCodeStore;
        private readonly IEnumerable<IDiscountCodeGenerator> discountCodeGenerators;

        public DiscountCodeWorkflows(
            ILogger<DiscountCodeWorkflows> logger,
            IDateTimeProvider dateTimeProvider,
            IIntegratedSystemStore integratedSystemStore,
            IDiscountCodeStore discountCodeStore,
            IEnumerable<IDiscountCodeGenerator> discountCodeGenerators)
        {
            this.logger = logger;
            this.dateTimeProvider = dateTimeProvider;
            this.integratedSystemStore = integratedSystemStore;
            this.discountCodeStore = discountCodeStore;
            this.discountCodeGenerators = discountCodeGenerators;
        }

        public async Task<IEnumerable<DiscountCode>> GenerateDiscountCodes(RecommendableItem promotion)
        {
            var discountCodes = new List<DiscountCode>();

            // Return empty list for unqualified promotions
            if (promotion.Id == RecommendableItem.DefaultRecommendableItem.Id ||
                promotion.PromotionType != PromotionType.Discount ||
                promotion.BenefitValue == 0)
            {
                return discountCodes;
            }

            var integratedSystems = (await integratedSystemStore.Query()).Items;
            var discountCodeGeneratorSystems = integratedSystems
                .Where(_ => _.IsDiscountCodeGenerator && _.IntegrationStatus == IntegrationStatuses.OK);

            bool willGenerate = discountCodeGenerators.Any(_ => discountCodeGeneratorSystems.Any(x => x.SystemType == _.SystemType));

            if (willGenerate)
            {
                DiscountCode discountCode = null;
                // Always generate a unique discount code
                if (discountCode == null)
                {
                    string code = string.Empty;
                    var startsAt = dateTimeProvider.Now;
                    var endsAt = startsAt.TruncateToDayStart().AddDays(14); // default 14 days
                    bool codeExists = false;
                    do
                    {
                        // Ensure that the new code does not yet exist in Cherry
                        // Code generation logic can be further optimized
                        code = DiscountCode.GenerateCode(promotion.CommonId, codeLength: 8);
                        var result = await discountCodeStore.ReadByCode(code);
                        codeExists = result.Success;
                    } while (codeExists);
                    discountCode = new DiscountCode(promotion, code, startsAt, endsAt);
                    discountCode = await discountCodeStore.Create(discountCode);
                    foreach (var integratedSystem in discountCodeGeneratorSystems)
                    {
                        var generator = discountCodeGenerators.FirstOrDefault(_ => _.SystemType == integratedSystem.SystemType);
                        if (generator != null)
                        {
                            // Discount code is passed to the generators so multiple generators will have the same code
                            await generator.Generate(integratedSystem, promotion, discountCode);
                            discountCode.GeneratedAt.Add(integratedSystem);
                        }
                        else
                        {
                            logger.LogWarning("No generator found for integrated system {integratedSystemId} with type {systemType}.", integratedSystem.Id, integratedSystem.SystemType);
                        }
                    }
                }

                if (discountCode != null)
                {
                    discountCodes.Add(discountCode);
                }
                else
                {
                    logger.LogWarning("Invalid discount code generation scenario. willGenerate {willGenerate} but discount code is null. ", willGenerate);
                }
            }

            return discountCodes;
        }

        public async Task LoadGeneratedAt(IEnumerable<DiscountCode> discountCodes)
        {
            foreach (var discountCode in discountCodes)
            {
                await discountCodeStore.LoadMany(discountCode, _ => _.GeneratedAt);
            }
        }
    }
}