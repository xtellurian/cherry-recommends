using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    [Obsolete("This entity or table is obsolete.")]
    public class EFTrackedUserActionStore : EFEntityStoreBase<TrackedUserAction>, ITrackedUserActionStore
    {
        protected override Expression<Func<TrackedUserAction, DateTimeOffset>> defaultOrderBy => _ => _.Timestamp;
        protected override int PageSize => 10;
        public EFTrackedUserActionStore(IDbContextProvider<SignalBoxDbContext> contextProvider)
        : base(contextProvider, c => c.TrackedUserActions)
        { }

        public async Task<TrackedUserAction> ReadLatestAction(string commonUserId, string category, string actionName)
        {
            if (category == null)
            {
                throw new NullReferenceException("category cannot be null when querying ReadLatestAction");
            }
            if (commonUserId == null)
            {
                throw new NullReferenceException("commonUserId cannot be null when querying ReadLatestAction");
            }
            if (actionName != null)
            {
                return await Set
                    .Where(_ => _.CommonUserId == commonUserId && _.Category == category && _.ActionName == actionName)
                    .OrderByDescending(_ => _.Timestamp)
                    .FirstAsync();
            }
            else
            {
                return await Set
                    .Where(_ => _.CommonUserId == commonUserId && _.Category == category)
                    .OrderByDescending(_ => _.Timestamp)
                    .FirstAsync();
            }
        }

        public async Task<Paginated<ActionCategoryAndName>> ReadTrackedUserCategoriesAndActionNames(int page, string commonUserId)
        {
            var itemCount = await Set
                .Where(_ => _.CommonUserId == commonUserId)
                .Select(_ => new { _.Category, _.ActionName })
                .Distinct()
                .CountAsync();

            IEnumerable<ActionCategoryAndName> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                var anonResults = await Set
                    .Where(_ => _.CommonUserId == commonUserId)
                    .OrderByDescending(_ => _.Timestamp)
                    .Select(_ => new { _.Category, _.ActionName })
                    .Distinct()
                    .OrderBy(_ => _.Category)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
                results = anonResults.Select(_ => new ActionCategoryAndName(_.Category, _.ActionName));
            }
            else
            {
                results = new List<ActionCategoryAndName>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<ActionCategoryAndName>(results, pageCount, itemCount, page);
        }

        public async Task<Paginated<string>> ReadAllUniqueActionNames(int page, string term)
        {
            Expression<Func<TrackedUserAction, bool>> predicate =
                _ => EF.Functions.Like(_.ActionName, $"%{term}%");

            var itemCount = await Set
                .Where(predicate)
                .Select(_ => _.ActionName)
                .Distinct()
                .CountAsync();

            List<string> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(predicate)
                    .OrderByDescending(_ => _.Timestamp)
                    .Select(_ => _.ActionName)
                    .Distinct()
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<string>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<string>(results, pageCount, itemCount, page);
        }

        public async Task<Paginated<ActionCategoryAndName>> ReadAllCategoriesWithActionNames(int page, string searchTerm = null)
        {
            Expression<Func<TrackedUserAction, bool>> predicate = _ => true;
            if (searchTerm != null)
            {
                // search in category and actionName
                predicate = _ => (EF.Functions.Like(_.ActionName, $"%{searchTerm}%") || EF.Functions.Like(_.Category, $"%{searchTerm}%"));
            }

            var itemCount = await Set
                .Where(predicate)
                .Select(_ => new { _.Category, _.ActionName })
                .Distinct()
                .CountAsync();

            IEnumerable<ActionCategoryAndName> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                var anonResults = await Set
                    .Where(predicate)
                    .OrderByDescending(_ => _.Timestamp)
                    .Select(_ => new { _.Category, _.ActionName })
                    .Distinct()
                    .OrderBy(_ => _.Category)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
                results = anonResults.Select(_ => new ActionCategoryAndName(_.Category, _.ActionName));
            }
            else
            {
                results = new List<ActionCategoryAndName>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<ActionCategoryAndName>(results, pageCount, itemCount, page);
        }
    }
}