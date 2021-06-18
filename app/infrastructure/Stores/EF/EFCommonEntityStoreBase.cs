using System;
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

        public virtual async Task<T> ReadFromCommonId<TProperty>(string commonId, Expression<Func<T, TProperty>> include)
        {
            try
            {
                return await Set.Include(include).FirstAsync(_ => _.CommonId == commonId);
            }
            catch (Exception ex)
            {
                throw new StorageException($"Failed to retreive {typeof(T)} with commonId {commonId}", ex);
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
                throw new StorageException($"Failed to retreive {typeof(T)} with commonId {commonId}", ex);
            }
        }

        public virtual async Task<string> FindCommonId(long id)
        {
            var entity = await Set.FindAsync(id);
            return entity.CommonId;
        }

    }
}