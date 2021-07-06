using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFCommonEntityStoreBase<T> : EFEntityStoreBase<T>, ICommonEntityStore<T> where T : CommonEntity
    {
        public EFCommonEntityStoreBase(SignalBoxDbContext context, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(context, selector)
        { }

        public virtual async Task<Paginated<T>> Query(int page, string searchTerm)
        {

            Expression<Func<T, bool>> predicate = 
                _ => EF.Functions.Like(_.CommonId, $"%{searchTerm}%") || EF.Functions.Like(_.Name, $"%{searchTerm}%");
            var itemCount = await Set.CountAsync(predicate);
            List<T> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await Set
                    .Where(predicate)
                    .OrderByDescending(_ => _.LastUpdated)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .ToListAsync();
            }
            else
            {
                results = new List<T>();
            }
            var pageCount = (int)Math.Ceiling((double)itemCount / PageSize);
            return new Paginated<T>(results, pageCount, itemCount, page);

        }

        public virtual async Task<T> ReadFromCommonId<TProperty>(string commonId, Expression<Func<T, TProperty>> include)
        {
            try
            {
                return await Set.Include(include).FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), commonId, ex);
            }
        }

        public async Task<bool> ExistsFromCommonId(string commonId)
        {
            return await this.Set.AnyAsync(_ => _.CommonId == commonId);
        }

        public virtual async Task<T> ReadFromCommonId(string commonId)
        {
            try
            {
                return await Set.FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), commonId, ex);
            }
        }

        public virtual async Task<string> FindCommonId(long id)
        {
            var entity = await Set.FindAsync(id);
            return entity.CommonId;
        }
    }
}