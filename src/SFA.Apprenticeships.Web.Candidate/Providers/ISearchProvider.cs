namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels;
    using ViewModels.VacancySearch;

    public interface ISearchProvider
    {
        LocationsViewModel FindLocation(string placeNameOrPostcode);

        AddressSearchResult FindAddresses(string postcode);

        ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search);

        TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search);
    }
}