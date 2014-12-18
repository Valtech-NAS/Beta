namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;

    [TestFixture]
    public class SearchProviderTests
    {
        private Mock<ILocationSearchService> _locationSearchService;
        private Mock<IVacancySearchService<ApprenticeshipSummaryResponse>> _apprenticeshipSearchService;
        private Mock<IVacancySearchService<TraineeshipSummaryResponse>> _traineeshipSearchService;
        private Mock<IAddressSearchService> _addressSearchService;
        private Mock<IPerformanceCounterService> _performanceCounterService;

        private ApprenticeshipCandidateWebMappers _apprenticeshipMapper;
        private TraineeshipCandidateWebMappers _traineeeshipMapper;
        
        [SetUp]
        public void Setup()
        {
            _locationSearchService = new Mock<ILocationSearchService>();
            _apprenticeshipSearchService = new Mock<IVacancySearchService<ApprenticeshipSummaryResponse>>();
            _traineeshipSearchService = new Mock<IVacancySearchService<TraineeshipSummaryResponse>>();
            _addressSearchService = new Mock<IAddressSearchService>();
            _performanceCounterService = new Mock<IPerformanceCounterService>();
            
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
                _performanceCounterService.Object);

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
                _performanceCounterService.Object);

            var results = searchProvider.FindLocation(string.Empty);

            results.Locations.Should().BeEmpty();
        }

        [TestCase]
        public void ShouldFindVacanciesFromCriteria()
        {
            const int pageSize = 10;
            var results = new 
                SearchResults<ApprenticeshipSummaryResponse>(100, 1, new List<ApprenticeshipSummaryResponse>
            {
                new ApprenticeshipSummaryResponse
                {
                    VacancyLocationType = ApprenticeshipLocationType.National
                }
            });

            _apprenticeshipSearchService.Setup(
                x => x.Search(It.IsAny<SearchParameters>())).Returns(results);          

            var search = new ApprenticeshipSearchViewModel
            {
                Location = "Test",
                Longitude = 0d,
                Latitude = 0d,
                PageNumber = 1,
                WithinDistance = 2,
                ResultsPerPage = pageSize,
                LocationType = ApprenticeshipLocationType.National
            };

            var searchProvider = new SearchProvider(_locationSearchService.Object,
                _apprenticeshipSearchService.Object,
                _traineeshipSearchService.Object,
                _addressSearchService.Object,
                _apprenticeshipMapper,
                _traineeeshipMapper,
                _performanceCounterService.Object);

            var test = searchProvider.FindVacancies(search);

            test.Should().NotBeNull();
            test.Pages.Should().Be(10);
            test.NextPage.Should().Be(2);
            test.PrevPage.Should().Be(0);
            test.TotalNationalHits.Should().Be(100);
            test.TotalLocalHits.Should().Be(0);
            test.VacancySearch.Should().Be(search);
        }
    }
}
