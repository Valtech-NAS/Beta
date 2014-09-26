namespace SFA.Apprenticeships.Application.Interfaces.Search
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "http://candidates.gov.uk")]
    public class SearchResults<TResult> where TResult : class
    {
        public SearchResults(long total, int pageNumber, IEnumerable<TResult> results)
        {
            Total = total;
            PageNumber = pageNumber;
            Results = results;
        }

        [DataMember(Order = 1, Name = "Total")]
        public long Total { get; private set; }

        [DataMember(Order = 2, Name = "PageNumber")]
        public int PageNumber { get; private set; }

        [DataMember(Order = 3, Name = "Results")]
        public IEnumerable<TResult> Results { get; private set; }
    }
}