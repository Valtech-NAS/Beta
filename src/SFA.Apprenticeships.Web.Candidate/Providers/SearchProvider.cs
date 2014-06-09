using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    public class SearchProvider : ISearchProvider
    {
        public IEnumerable<LookupLocation> FindLocation(string placeNameOrPostcode)
        {
            throw new NotImplementedException();
        }
    }
}