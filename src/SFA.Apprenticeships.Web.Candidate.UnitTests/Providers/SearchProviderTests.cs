namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SearchProviderTests
    {
        private const int PageSize = 10;

        private Mock<ILogService> _logService;
        private Mock<ILocationSearchService> _locationSearchService;
        private Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>> _apprenticeshipSearchService;
        private Mock<IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>> _traineeshipSearchService;
        private Mock<IAddressSearchService> _addressSearchService;

        private ApprenticeshipCandidateWebMappers _apprenticeshipMapper;
        private TraineeshipCandidateWebMappers _traineeeshipMapper;
        
        [SetUp]
        public void Setup()
        {
            _logService = new Mock<ILogService>();
            _locationSearchService = new Mock<ILocationSearchService>();
            _apprenticeshipSearchService = new Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            _traineeshipSearchService = new Mock<IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>();
            _addressSearchService = new Mock<IAddressSearchService>();
            
            _apprenticeshipMapper = new ApprenticeshipCandidateWebMappers();
            _traineeeshipMapper = new TraineeshipCandidateWebMappers();
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromLocation()
        {
            var locations = new List<Location>
            {
                new Location {Name = "Location1", GeoPoint = new GeoPoint {Latitude = 0.1d, Longitude = 0.2d}}
            };

            _locationSearchService.Setup(x => x.FindLocation("Location1")).Returns(locations);

            var searchProvider = new SearchProvider(_locationSearchService.Object, 
                _apprenticeshipSearchService.Object,
                _traineeshipSearchService.Object,
                _addressSearchService.Object, 
                _apprenticeshipMapper, 
                _traineeeshipMapper,
                _logService.Object);

            var results = searchProvider.FindLocation("Location1");
            var result = results.Locations.First();

            result.Should().NotBeNull();
            result.Latitude.Should().Be(0.1d);
            result.Longitude.Should().Be(0.2d);
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromNullLocation()
        {
            _locationSearchService.Setup(x => x.FindLocation(It.IsAny<string>()))
                .Returns(default(IEnumerable<Location>));

            var searchProvider = new SearchProvider(_locationSearchService.Object,
                _apprenticeshipSearchService.Object,
                _traineeshipSearchService.Object,
                _addressSearchService.Object,
                _apprenticeshipMapper,
                _traineeeshipMapper,
                _logService.Object);

            var results = searchProvider.FindLocation(string.Empty);

            results.Locations.Should().BeEmpty();
        }

        [TestCase]
        public void ShouldFindVacanciesFromCriteria()
        {
            SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType.National);

            var search = GetASearchViewModelOfType(ApprenticeshipLocationType.National);

            var searchProvider = GetSearchProvider();

            var test = searchProvider.FindVacancies(search);

            test.Should().NotBeNull();
            test.Pages.Should().Be(10);
            test.NextPage.Should().Be(2);
            test.PrevPage.Should().Be(0);
            test.TotalNationalHits.Should().Be(100);
            test.TotalLocalHits.Should().Be(0);
            test.VacancySearch.Should().Be(search);
        }

        [Test]
        public void IfThereIsntNonNationalResultsButThereAreNationalResuts_ShouldReturnLocationTypeAsNational()
        {
            SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType.National);

            var search = GetASearchViewModelOfType(ApprenticeshipLocationType.NonNational);

            var searchProvider = GetSearchProvider();
            
            var vacancies = searchProvider.FindVacancies(search);

            vacancies.VacancySearch.LocationType.Should().Be(ApprenticeshipLocationType.National);
        }

        [Test]
        public void
            IfItsANationalSearchButThereIsntNationalResuls_TheNonNationalResultsAreReturned()
        {
            SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType.NonNational);

            var search = GetASearchViewModelOfType(ApprenticeshipLocationType.National);

            var searchProvider = GetSearchProvider();

            var vacancies = searchProvider.FindVacancies(search);

            vacancies.Vacancies.Should().HaveCount(1);
            vacancies.Vacancies.First().VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        private static ApprenticeshipSearchViewModel GetASearchViewModelOfType(ApprenticeshipLocationType locationType)
        {
            var search = new ApprenticeshipSearchViewModel
            {
                Location = "Test",
                Longitude = 0d,
                Latitude = 0d,
                PageNumber = 1,
                WithinDistance = 2,
                ResultsPerPage = PageSize,
                LocationType = locationType
            };
            return search;
        }

        private void SetupReturnOneHundredResultsOfType(ApprenticeshipLocationType locationType)
        {
            _apprenticeshipSearchService.Setup(
                x => x.Search(It.Is<ApprenticeshipSearchParameters>(asp => asp.VacancyLocationType == locationType))).Returns<ApprenticeshipSearchParameters>(asp => new
                SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>(100, 1, new List<ApprenticeshipSearchResponse>
                {
                    new ApprenticeshipSearchResponse
                    {
                        VacancyLocationType = locationType
                    }
                }, new List<AggregationResult>(0), asp));

            _apprenticeshipSearchService.Setup(
                x => x.Search(It.Is<ApprenticeshipSearchParameters>(asp => asp.VacancyLocationType != locationType))).Returns<ApprenticeshipSearchParameters>(asp => new
                SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>(0, 1, new List<ApprenticeshipSearchResponse>
                {
                    new ApprenticeshipSearchResponse
                    {
                        VacancyLocationType = locationType
                    }
                }, new List<AggregationResult>(0), asp));
        }

        private SearchProvider GetSearchProvider()
        {
            var searchProvider = new SearchProvider(_locationSearchService.Object,
                _apprenticeshipSearchService.Object,
                _traineeshipSearchService.Object,
                _addressSearchService.Object,
                _apprenticeshipMapper,
                _traineeeshipMapper,
                _logService.Object);
            return searchProvider;
        }
    }
}
