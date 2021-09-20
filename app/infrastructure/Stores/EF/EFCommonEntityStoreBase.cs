using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public abstract class EFCommonEntityStoreBase<T> : EFEnvironmentScopedEntityBase<T>, ICommonEntityStore<T> where T : CommonEntity
    {
        public EFCommonEntityStoreBase(SignalBoxDbContext context, IEnvironmentService environmentService, Func<SignalBoxDbContext, DbSet<T>> selector)
        : base(context, environmentService, selector)
        { }

        public override async Task<T> Create(T entity)
        {
            if (await Set.AnyAsync(_ => _.CommonId == entity.CommonId))
            {
                throw new StorageException($"CommonId {entity.CommonId} of type {typeof(T).Name} already exists in the database.");
            }
            else
                return await base.Create(entity);
        }

        public virtual async Task<Paginated<T>> Query(int page, string searchTerm)
        {

            Expression<Func<T, bool>> predicate =
                _ => EF.Functions.Like(_.CommonId, $"%{searchTerm}%") || EF.Functions.Like(_.Name, $"%{searchTerm}%");
            var itemCount = await QuerySet.CountAsync(predicate);
            List<T> results;

            if (itemCount > 0) // check and let's see whether the query is worth running against the database
            {
                results = await QuerySet
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
                return await QuerySet
                    .Include(include)
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), commonId, ex);
            }
        }

        public async Task<bool> ExistsFromCommonId(string commonId)
        {
            return await this.QuerySet
                .AnyAsync(_ => _.CommonId == commonId);
        }

        public virtual async Task<T> ReadFromCommonId(string commonId)
        {
            try
            {
                return await QuerySet
                    .FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(typeof(T), commonId, ex);
            }
        }
    }
}