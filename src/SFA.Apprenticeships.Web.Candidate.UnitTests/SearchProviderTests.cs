
namespace SFA.Apprenticeships.Web.Candidate.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Location;
    using SFA.Apprenticeships.Web.Candidate.Mappers;
    using SFA.Apprenticeships.Web.Candidate.Providers;

    [TestFixture]
    public class SearchProviderTests
    {
        private Mock<ILocationSearchService> _locationSearchService;
        private Mock<IVacancySearchProvider> _vacancySearchProvider;

        private CandidateWebMappers _mapper;

        [SetUp]
        public void Setup()
        {
            _locationSearchService = new Mock<ILocationSearchService>();
            _vacancySearchProvider = new Mock<IVacancySearchProvider>();
            _mapper = new CandidateWebMappers();
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromLocation()
        {
            var locations = new List<Location>
            {
                new Location {Name = "Location1", GeoPoint = new GeoPoint {Latitute = 0.1d, Longitude = 0.2d}}
            };

            _locationSearchService.Setup(x => x.FindLocation("Location1")).Returns(locations);

            var searchProvider = new SearchProvider(_locationSearchService.Object, _vacancySearchProvider.Object, _mapper);
            var test = searchProvider.FindLocation("Location1");
            var result = test.First();

            result.Should().NotBeNull();
            result.Latitude.Should().Be(0.1d);
            result.Longitude.Should().Be(0.2d);
        }

        [TestCase]
        public void ShouldReturnLocationViewModelsFromNullLocation()
        {
            _locationSearchService.Setup(x => x.FindLocation(It.IsAny<string>())).Returns(default(IEnumerable<Location>));

            var searchProvider = new SearchProvider(_locationSearchService.Object, _vacancySearchProvider.Object, _mapper);
            var test = searchProvider.FindLocation(string.Empty);

            test.Should().BeEmpty();
        }
    }
}
