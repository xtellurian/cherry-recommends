using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalBox.Core
{
    public interface ICommonEntityStore<T> : IEntityStore<T> where T : CommonEntity
    {
        Task<T> ReadFromCommonId(string commonId);
        Task<bool> ExistsFromCommonId(string commonId);
        Task<T> ReadFromCommonId<TProperty>(string commonId, Expression<Func<T, TProperty>> include);

        /// <summary>
        /// A safer method that explicity uses the environment ID to retreive the entity.
        /// </summary>
        /// <param name="commonId">The common ID to retrieve</param>
        /// <param name="environmentId">The environment in which to check.</param>
        /// <returns>True if the common ID exists in the environment</returns>
        Task<T> ReadFromCommonId<TProperty>(string commonId, long? environmentId, Expression<Func<T, TProperty>> include);
        /// <summary>
        /// A safer method that explicity checks the environment ID for an existing common entity.
        /// </summary>
        /// <param name="commonId">The common ID to check</param>
        /// <param name="environmentId">The environment in which to check.</param>
        /// <returns>True if the common ID exists in the environment</returns>
        Task<bool> ExistsFromCommonId(string commonId, long? environmentId);
    }
}