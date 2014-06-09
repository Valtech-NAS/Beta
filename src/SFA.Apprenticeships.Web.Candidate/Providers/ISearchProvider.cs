namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public interface ISearchProvider
    {
        IEnumerable<LocationViewModel> FindLocation(string placeNameOrPostcode);

        VacancySearchResponseViewModel FindVacancies(LocationViewModel location, int radius);
    }
}
