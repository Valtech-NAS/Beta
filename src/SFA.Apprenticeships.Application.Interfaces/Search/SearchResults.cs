namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using System.Collections.Generic;

    public class SearchResults<TResult> where TResult : class
    {
        public SearchResults(int total, IEnumerable<TResult> results)
        {
            Total = total;
            Results = results;
        }

        public int Total { get; private set; }

        public IEnumerable<TResult> Results { get; private set; } 
    }
}
