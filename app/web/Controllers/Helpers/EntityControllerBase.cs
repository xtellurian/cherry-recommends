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
            return await store.Query(p.Page);
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public virtual async Task<T> GetEntity(long id)
        {
            return await store.Read(id);
        }
    }
}