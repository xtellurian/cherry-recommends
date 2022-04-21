using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalBox.Core.Workflows
{
    public class DiscountCodeWorkflows : IDiscountCodeWorkflow, IWorkflow
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IIntegratedSystemStore integratedSystemStore;
        private readonly IDiscountCodeStore discountCodeStore;
        private readonly IEnumerable<IDiscountCodeGenerator> discountCodeGenerators;

        public DiscountCodeWorkflows(
            IDateTimeProvider dateTimeProvider,
            IIntegratedSystemStore integratedSystemStore,
            IDiscountCodeStore discountCodeStore,
            IEnumerable<IDiscountCodeGenerator> discountCodeGenerators)
        {
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
                string code = DiscountCode.GenerateCode(promotion.CommonId, codeLength: 8);
                var startsAt = dateTimeProvider.Now;
                var endsAt = startsAt.TruncateToDayStart().AddDays(14); // default 14 days
                var discountCode = new DiscountCode(promotion, code, startsAt, endsAt);
                var result = await discountCodeStore.GetLatestByPromotion(promotion);

                if (result.Success)
                {
                    // Re-use valid generated discount code created within the day
                    // Entity Created property is in UTC
                    if (result.Entity.WasCreatedOnDate(dateTimeProvider.Now.UtcDateTime) &&
                        result.Entity.IsActiveByDate(dateTimeProvider.Now))
                    {
                        discountCode = result.Entity;
                        await discountCodeStore.LoadMany(discountCode, _ => _.GeneratedAt);
                    }
                }
                else
                {
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
                    }
                }

                discountCodes.Add(discountCode);
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