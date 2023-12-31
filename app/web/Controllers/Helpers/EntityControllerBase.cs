using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalBox.Core;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    public class EntityControllerBase<T> : SignalBoxControllerBase where T : Entity
    {
        protected IEntityStore<T> store { get; }

        public EntityControllerBase(IEntityStore<T> store)
        {
            this.store = store;
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public virtual async Task<Paginated<T>> Query([FromQuery] PaginateRequest p)
        {
            return await store.Query(p);
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public virtual async Task<T> GetResource(long id)
        {
            return await store.Read(id, new EntityStoreReadOptions { ChangeTracking = ChangeTrackingOptions.NoTracking });
        }

        /// <summary>Deletes the resource with this Id.</summary>
        [HttpDelete("{id}")]
        public virtual async Task<DeleteResponse> DeleteResource(long id)
        {
            var result = await store.Remove(id);
            await store.Context.SaveChanges();
            return new DeleteResponse(id, Request.Path.Value, result);
        }
    }
}