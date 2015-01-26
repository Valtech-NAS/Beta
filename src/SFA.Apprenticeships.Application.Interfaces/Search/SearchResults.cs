namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using System.Collections.Generic;

    public class SearchResults<TResult> where TResult : class
    {
        public SearchResults(long total, int pageNumber, IEnumerable<TResult> results, IEnumerable<AggregationResult> aggregationResults )
        {
            Total = total;
            PageNumber = pageNumber;
            Results = results;
            AggregationResults = aggregationResults;
        }

        public long Total { get; private set; }

        public int PageNumber { get; private set; }

        public IEnumerable<TResult> Results { get; private set; }

        public IEnumerable<AggregationResult> AggregationResults { get; private set; }
    }
}
