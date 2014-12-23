namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Constants.ViewModels;
    using ViewModels.VacancySearch;

    public interface ISearchProvider
    {
        LocationsViewModel FindLocation(string placeNameOrPostcode);

        AddressSearchResult FindAddresses(string postcode);

        ApprenticeshipSearchResponseViewModel FindVacancies(ApprenticeshipSearchViewModel search);

        TraineeshipSearchResponseViewModel FindVacancies(TraineeshipSearchViewModel search);
    }
}