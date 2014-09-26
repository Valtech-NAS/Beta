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
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SearchProviderTests
    {
        private Mock<ILocationSearchService> _locationSearchService;
        private Mock<IVacancySearchService> _vacancySearchService;
        private Mock<IAddressSearchService> _addressSearchService;

        private CandidateWebMappers _mapper;
        
        [SetUp]
        public void Setup()
        {
            _locationSearchService = new Mock<ILocationSearchService>();
            _vacancySearchService = new Mock<IVacancySearchService>();
            _addressSearchService = new Mock<IAddressSearchService>();
            _mapper = new CandidateWebMappers();
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromLocation()
        {
            var locations = new List<Location>
            {
                new Location {Name = "Location1", GeoPoint = new GeoPoint {Latitude = 0.1d, Longitude = 0.2d}}
            };

            _locationSearchService.Setup(x => x.FindLocation("Location1")).Returns(locations);

            var searchProvider = new SearchProvider(_locationSearchService.Object, _vacancySearchService.Object, _addressSearchService.Object, _mapper);
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

            var searchProvider = new SearchProvider(_locationSearchService.Object, _vacancySearchService.Object, _addressSearchService.Object, _mapper);
            var results = searchProvider.FindLocation(string.Empty);

            results.Locations.Should().BeEmpty();
        }

        [TestCase]
        public void ShouldFindVacanciesFromCriteria()
        {
            const int pageSize = 10;
            var results = new SearchResults<VacancySummaryResponse>(100, 1, new List<VacancySummaryResponse>());

            _vacancySearchService.Setup(
                x => x.Search(new SearchParameters
                {
                    Keywords = It.IsAny<string>(),
                    Location = It.IsAny<Location>(),
                    PageNumber = 1,
                    PageSize = pageSize,
                    SearchRadius = It.IsAny<int>(),
                    SortType = VacancySortType.Relevancy,
                    VacancyLocationType = VacancyLocationType.National
                })).Returns(results);

            _vacancySearchService.Setup(
                x => x.Search(new SearchParameters
                {
                    Keywords = It.IsAny<string>(),
                    Location = It.IsAny<Location>(),
                    PageNumber = 1,
                    PageSize = pageSize,
                    SearchRadius = It.IsAny<int>(),
                    SortType = VacancySortType.Relevancy,
                    VacancyLocationType = VacancyLocationType.NonNational
                })).Returns(results);

            var search = new VacancySearchViewModel
            {
                Location = "Test",
                Longitude = 0d,
                Latitude = 0d,
                PageNumber = 1,
                WithinDistance = 2,
                ResultsPerPage = pageSize,
                LocationType = VacancyLocationType.National
            };

            var searchProvider = new SearchProvider(_locationSearchService.Object, _vacancySearchService.Object, _addressSearchService.Object, _mapper);
            var test = searchProvider.FindVacancies(search);

            test.Should().NotBeNull();
            test.Pages.Should().Be(10);
            test.NextPage.Should().Be(2);
            test.PrevPage.Should().Be(1);
            test.TotalLocalHits.Should().Be(100);
            test.VacancySearch.Should().Be(search);
        }
    }
}
