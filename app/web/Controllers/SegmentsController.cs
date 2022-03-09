using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.Workflows;
using SignalBox.Web.Dto;

namespace SignalBox.Web.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("0.1")]
    [Route("api/[controller]")]
    public class SegmentsController : EntityControllerBase<SignalBox.Core.Segment>
    {
        private readonly ILogger<SegmentsController> logger;
        private readonly CustomerSegmentWorkflows workflow;
        private readonly ICustomerStore customerStore;

        public SegmentsController(ILogger<SegmentsController> logger, CustomerSegmentWorkflows workflow, ISegmentStore store, ICustomerStore customerStore) : base(store)
        {
            this.logger = logger;
            this.workflow = workflow;
            this.customerStore = customerStore;
        }

        /// <summary>Creates a new segment.</summary>
        [HttpPost]
        public async Task<SignalBox.Core.Segment> CreateSegment([FromBody] CreateSegmentDto dto)
        {
            return await workflow.CreateSegment(dto.Name);
        }

        /// <summary>Update the name of this resource.</summary>
        [HttpPost("{id}/Name")]
        public virtual async Task<SignalBox.Core.Segment> Rename(long id, string name)
        {
            var segment = await store.Read(id);
            segment.Name = name;
            await store.Update(segment);
            await store.Context.SaveChanges();
            return segment;
        }

        /// <summary>Add a customer to a segment.</summary>
        [HttpPost("{id}/Customers/{customerId}")]
        public virtual async Task<Customer> AddCustomer(long id, long customerId)
        {
            var segment = await store.Read(id); ;
            var customer = await customerStore.Read(customerId);
            await workflow.AddToSegment(segment, customer);
            return customer;
        }

        /// <summary>Remove a customer from a segment.</summary>
        [HttpDelete("{id}/Customers/{customerId}")]
        public virtual async Task<Customer> RemoveCustomer(long id, long customerId)
        {
            var segment = await store.Read(id); ;
            var customer = await customerStore.Read(customerId);
            await workflow.RemoveFromSegment(segment, customer);
            return customer;
        }

        /// <summary>Gets the Customers that are in a segment.</summary>
        [HttpGet("{id}/Customers")]
        public async Task<Paginated<Customer>> GetInSegment(string id, [FromQuery] PaginateRequest p, [FromQuery] SearchEntities q)
        {
            if (long.TryParse(id, out var segmentId))
            {
                List<Expression<Func<Customer, bool>>> expressions = new List<Expression<Func<Customer, bool>>>();
                Expression<Func<Customer, bool>> predicate = null;

                expressions.Add(_ => _.Segments.Any(s => s.SegmentId == segmentId));

                if (!string.IsNullOrEmpty(q.Term))
                {
                    expressions.Add(_ => EF.Functions.Like(_.CommonId, $"%{q.Term}%") || EF.Functions.Like(_.Name, $"%{q.Term}%"));
                }
                if (q.WeeksAgo.HasValue)
                {
                    expressions.Add(_ => EF.Functions.DateDiffWeek(_.Created, DateTime.UtcNow) <= q.WeeksAgo.Value);
                }

                foreach (var expression in expressions)
                {
                    predicate = predicate != null ? predicate.And(expression) : expression;
                }

                return await customerStore.Query(_ => _.Segments, new EntityStoreQueryOptions<Customer>(p.Page, predicate));
            }
            else
            {
                var results = Enumerable.Empty<Customer>();
                return new Paginated<Customer>(results, 0, 0, 1);
            }
        }
    }
}
