namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummaryProxy;

    [TestFixture]
    public class MapperTests
    {
        private VacancySummaryMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new VacancySummaryMapper();
        }

        [TestCase]
        public void ShouldCreateAMap()
        {
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [TestCase]
        public void ShouldReturnGeoPointFromAddressResolver()
        {
            var data = new VacancySummaryData
            {
                VacancyReference = 1,
                VacancyAddress = new AddressData
                {
                    Latitude = 12.9m,
                    Longitude = 0.18m,
                }
            };

           var test = _mapper.Map<VacancySummaryData, VacancySummary>(data);

            test.Id.Should().Be(1);
            test.Location.Latitute.Should().Be(12.9);
            test.Location.Longitude.Should().Be(0.18);
        }

        [TestCase]
        public void ShouldReturnEnumsFromEnumResolver()
        {
            var data = new VacancySummaryData
            {
                VacancyReference = 1,
                VacancyLocationType = "MultipleLocation",
                VacancyType = "IntermediateLevelApprenticeship",
            };

            var test = _mapper.Map<VacancySummaryData, VacancySummary>(data);

            test.Id.Should().Be(1);
            test.VacancyLocationType.Should().Be(VacancyLocationType.NonNational);
            test.VacancyType.Should().Be(VacancyType.Intermediate);
        }

        [TestCase]
        public void ShouldMapServiceCollectionToSummaryCollection()
        {
            var data = new[]
            {
                new VacancySummaryData
                {
                    VacancyReference = 1,
                    ApprenticeshipFramework = "TestFramework"
                }
            };

            var test = _mapper.Map<VacancySummaryData[], IEnumerable<VacancySummary>>(data).ToList();

            test[0].Id.Should().Be(1);
            test[0].Framework.Should().Be("TestFramework");
        }
    }
}
