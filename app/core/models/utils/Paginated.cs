using System.Collections.Generic;

namespace SignalBox.Core
{
#nullable enable
    public class Paginated<T> where T : class
    {
        public Paginated(IEnumerable<T> items, int pageCount, int totalItemCount, int pageNumber)
        {
            Items = items;
            Pagination = new PaginationInfo(pageCount, totalItemCount, pageNumber);
        }

        public Paginated(IEnumerable<T> items, string? after)
        {
            Items = items;
            Pagination = new PaginationInfo(new NextPageInfo(after));
        }

        public IEnumerable<T> Items { get; }

        public PaginationInfo Pagination { get; }
    }
}