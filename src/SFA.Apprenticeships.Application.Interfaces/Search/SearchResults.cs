namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using System.Collections.Generic;

    public class SearchResults<TResult> where TResult : class
    {
        public SearchResults(int total, int pageNumber, IEnumerable<TResult> results)
        {
            Total = total;
            PageNumber = pageNumber;
            Results = results;
        }

        public int Total { get; private set; }
        public int PageNumber { get; private set; }

        public IEnumerable<TResult> Results { get; private set; } 
    }
}
