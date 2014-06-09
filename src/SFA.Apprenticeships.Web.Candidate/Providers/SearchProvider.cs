
namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        public SearchProvider()
        {
            
        }

        public IEnumerable<LocationViewModel> FindLocation(string placeNameOrPostcode)
        {
            throw new NotImplementedException();
        }
    }
}