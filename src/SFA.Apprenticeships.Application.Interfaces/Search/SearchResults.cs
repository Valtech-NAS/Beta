namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using System.Collections.Generic;

    public class SearchResults<TResult, TSearchParameters>
        where TResult : class
        where TSearchParameters : SearchParametersBase
    {
        public SearchResults(long total, int pageNumber, IEnumerable<TResult> results, IEnumerable<AggregationResult> aggregationResults, TSearchParameters searchParameters)
        {
            Total = total;
            PageNumber = pageNumber;
            Results = results;
            AggregationResults = aggregationResults;
            SearchParameters = searchParameters;
        }

        public long Total { get; private set; }

        //TODO DFSW This is not required now that we are returning the initial request
        public int PageNumber { get; private set; }

        public IEnumerable<TResult> Results { get; private set; }

        public IEnumerable<AggregationResult> AggregationResults { get; private set; }

        public TSearchParameters SearchParameters { get; private set; }
    }
}
