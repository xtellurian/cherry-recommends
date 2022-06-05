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


        /// <summary>
        /// Query entities of this type.
        /// </summary>
        /// <param name="p">Controls pagination.</param>
        /// <param name="q">Controls querying</param>
        /// <returns>A paginated collection of entities.</returns>
        [HttpGet]
        public virtual async Task<Paginated<T>> Query([FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            if (string.IsNullOrEmpty(q.Term))
            {
                return await store.Query(p);
            }
            else
            {
                return await store.NoTrackingQuery(p, q.Term);
            }
        }

        /// <summary>Returns the resource with this Id.</summary>
        [HttpGet("{id}")]
        public virtual async Task<T> GetResource(string id, bool? useInternalId = null)
        {
            return await GetEntity<T>(id, useInternalId);
        }

        /// <summary>Update the name of this the resource.</summary>
        [HttpPost("{id}/name")]
        public virtual async Task<T> Rename(string id, string name, bool? useInternalId = null)
        {
            var entity = await GetEntity<T>(id, useInternalId);
            entity.Name = name;
            await store.Update(entity);
            await store.Context.SaveChanges();
            return entity;
        }

        /// <summary>Get the properties associated with this resource.</summary>
        [HttpGet("{id}/Properties")]
        public virtual async Task<DynamicPropertyDictionary> GetProperties(string id, bool? useInternalId = null)
        {
            var entity = await GetEntity<T>(id, useInternalId);
            return entity.Properties;
        }

        /// <summary>Set the properties associated with this resource.</summary>
        [HttpPost("{id}/Properties")]
        public virtual async Task<DynamicPropertyDictionary> SetProperties(string id, [FromBody] DynamicPropertyDictionary properties, bool? useInternalId = null)
        {
            var entity = await GetEntity<T>(id, useInternalId);
            properties.Validate();
            entity.Properties = properties;
            await store.Update(entity);
            await store.Context.SaveChanges();
            return entity.Properties;
        }

        protected async Task<T> GetEntity(string id, bool? useInternalId)
        {
            // wrapper around the generic, without an include selector
            return await this.GetEntity<object>(id, useInternalId);
        }

        protected async Task<T> GetEntity<TProperty>(string id, bool? useInternalId)
        {
            return await store.GetEntity(id, useInternalId);
        }

        protected virtual Task WillDelete(T entity)
        {
            return Task.CompletedTask;
        }

        /// <summary>Deletes the resource with this Id.</summary>
        [HttpDelete("{id}")]
        public virtual async Task<DeleteResponse> DeleteResource(long id)
        {
            var entity = await store.Read(id);
            var (canDelete, message) = await CanDelete(entity);
            if (canDelete)
            {
                await WillDelete(entity);
                var result = await store.Remove(id);
                await store.Context.SaveChanges();
                return new DeleteResponse(id, Request.Path.Value, result);
            }
            else
            {
                throw new BadRequestException($"Delete error: {message}", message);
            }
        }
    }
}