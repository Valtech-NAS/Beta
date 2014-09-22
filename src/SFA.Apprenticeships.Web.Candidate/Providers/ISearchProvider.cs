namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Constants.ViewModels;
    using ViewModels.VacancySearch;

    public interface ISearchProvider
    {
        LocationsViewModel FindLocation(string placeNameOrPostcode);

        AddressSearchResult FindAddresses(string postcode);

        VacancySearchResponseViewModel FindVacancies(VacancySearchViewModel search);

        bool IsValidPostcode(string postcode);
    }
}