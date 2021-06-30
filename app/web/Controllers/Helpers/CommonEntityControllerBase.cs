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
    public class CommonEntityControllerBase<T> : SignalBoxControllerBase where T : CommonEntity
    {
        protected ICommonEntityStore<T> store { get; }

        public CommonEntityControllerBase(ICommonEntityStore<T> store)
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
        public virtual async Task<T> GetResource(string id, bool? useInternalId = null)
        {
            if ((useInternalId == null || useInternalId == true) && int.TryParse(id, out var internalId))
            {
                return await store.Read(internalId);
            }
            else if (useInternalId == true)
            {
                throw new BadRequestException("Internal Ids must be integers");
            }
            else
            {
                return await store.ReadFromCommonId(id);
            }
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