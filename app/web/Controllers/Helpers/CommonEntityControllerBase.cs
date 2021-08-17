using System;
using System.Linq.Expressions;
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
    public abstract class CommonEntityControllerBase<T> : SignalBoxControllerBase where T : CommonEntity
    {
        protected ICommonEntityStore<T> store { get; }
        protected abstract Task<(bool, string)> CanDelete(T entity);

        public CommonEntityControllerBase(ICommonEntityStore<T> store)
        {
            this.store = store;
        }

        /// <summary>Returned a paginated list of items for this resource.</summary>
        [HttpGet]
        public virtual async Task<Paginated<T>> Query([FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            if (string.IsNullOrEmpty(q.Term))
            {
                return await store.Query(p.Page);
            }
            else
            {
                return await store.Query(p.Page, q.Term);
            }
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public virtual async Task<T> GetResource(string id, bool? useInternalId = null)
        {
            return await GetEntity<T>(id, useInternalId, null);
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpPost("{id}/name")]
        public virtual async Task<T> Rename(string id, string name, bool? useInternalId = null)
        {
            var entity = await GetEntity<T>(id, useInternalId, null);
            entity.Name = name;
            await store.Update(entity);
            await store.Context.SaveChanges();
            return entity;
        }

        protected async Task<T> GetEntity(string id, bool? useInternalId)
        {
            // wrapper around the generic, without an include selector
            return await this.GetEntity<object>(id, useInternalId, null);
        }

        protected async Task<T> GetEntity<TProperty>(string id, bool? useInternalId, Expression<Func<T, TProperty>> include = null)
        {
            return await store.GetEntity(id, useInternalId);
        }

        /// <summary>Deletes the resource with this Id.</summary>
        [HttpDelete("{id}")]
        public virtual async Task<DeleteResponse> DeleteResource(long id)
        {
            var entity = await store.Read(id);
            var (canDelete, message) = await CanDelete(entity);
            if (canDelete)
            {
                var result = await store.Remove(id);
                await store.Context.SaveChanges();
                return new DeleteResponse(id, Request.Path.Value, result);
            }
            else
            {
                throw new BadRequestException("Delete error", message);
            }
        }
    }
}