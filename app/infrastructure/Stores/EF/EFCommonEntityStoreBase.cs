using System;
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

        public async Task<bool> ExistsFromCommonId(string commonId)
        {
            return await this.Set.AnyAsync(_ => _.CommonId == commonId);
        }

        public virtual async Task<T> ReadFromCommonId(string commonId)
        {
            return await Set.FirstAsync(_ => _.CommonId == commonId);
        }

        public virtual async Task<string> FindCommonId(long id)
        {
            var entity = await Set.FindAsync(id);
            return entity.CommonId;
        }

    }
}