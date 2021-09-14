using System.Text.Json.Serialization;

namespace SignalBox.Core
{
    public class PaginationInfo
    {
#nullable enable
        public PaginationInfo(int pageCount, int totalItemCount, int pageNumber)
        {
            PageCount = pageCount;
            TotalItemCount = totalItemCount;
            PageNumber = pageNumber;
        }

        public PaginationInfo(NextPageInfo next)
        {
            Next = next;
        }

        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        /// One-based index of this subset within the superset, zero if the superset is empty.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
        /// is NOT the first subset within the superset.
        /// </summary>
        public bool HasPreviousPage => TotalItemCount > 0 && PageNumber <= PageCount && !IsFirstPage;

        /// <summary>
        /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
        /// is NOT the last subset within the superset.
        /// </summary>
        public bool HasNextPage => TotalItemCount > 0 && PageNumber <= PageCount && !IsLastPage;

        /// <summary>
        /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
        /// is the first subset within the superset.
        /// </summary>
        public bool IsFirstPage => TotalItemCount > 0 && PageNumber <= PageCount && PageNumber == 1;

        /// <summary>
        /// Returns true if the superset is not empty and PageNumber is less than or equal to PageCount and this
        /// is the last subset within the superset.
        /// </summary>
        public bool IsLastPage => TotalItemCount > 0 && PageNumber == PageCount;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public NextPageInfo? Next { get; set; }
    }
}