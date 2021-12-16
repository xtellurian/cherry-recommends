using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ITrackedUserActionStore : IEntityStore<TrackedUserAction>
    {
        Task<TrackedUserAction> ReadLatestAction(string customerId, string category, string actionName);
        Task<Paginated<ActionCategoryAndName>> ReadTrackedUserCategoriesAndActionNames(int page, string customerId);
        Task<Paginated<string>> ReadAllUniqueActionNames(int page, string term);
        Task<Paginated<ActionCategoryAndName>> ReadAllCategoriesWithActionNames(int page, string searchTerm = null);
    }
}