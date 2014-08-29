namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System.Collections.Generic;
    using ViewModels.Locations;
    using ViewModels.VacancySearch;

    public interface ISearchProvider
    {
        LocationsViewModel FindLocation(string placeNameOrPostcode);

        IEnumerable<AddressViewModel> FindAddresses(string postcode);

        VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search, int pageSize);

        bool IsValidPostcode(string postcode);
    }
}
