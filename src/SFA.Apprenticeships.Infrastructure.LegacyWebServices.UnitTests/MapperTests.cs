using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using Mappers;
    using VacancyDetailProxy;
    using AddressData = VacancySummaryProxy.AddressData;
    using VacancySummaryData = VacancySummaryProxy.VacancySummaryData;

    [TestFixture]
    public class MapperTests
    {
        private VacancySummaryMapper _vacancySummaryMapper;
        private VacancyDetailMapper _vacancyDetailMapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _vacancySummaryMapper = new VacancySummaryMapper();
            _vacancyDetailMapper = new VacancyDetailMapper();
        }

        [TestCase]
        public void ShouldCreateAMap()
        {
            _vacancySummaryMapper.Mapper.AssertConfigurationIsValid();
            _vacancyDetailMapper.Mapper.AssertConfigurationIsValid();
        }

        [TestCase]
        public void ShouldReturnEnumsFromEnumResolver()
        {
            var data = new VacancyFullData
            {
                VacancyReference = 1,
                VacancyLocationType = "MultipleLocation",
                VacancyType = "IntermediateLevelApprenticeship",
            };

            var test = _vacancyDetailMapper.Map<VacancyFullData, Domain.Entities.Vacancies.VacancyDetail>(data);

            test.Id.Should().Be(1);
            test.VacancyLocationType.Should().Be(VacancyLocationType.NonNational);
            test.VacancyType.Should().Be(VacancyType.Intermediate);
        }

        [TestCase]
        public void ShouldMapAllProperties()
        {
            var data = new[]
            {
                new VacancySummaryData
                {
                    VacancyReference = 1,
                    EmployerName = "EmpName",
                    ClosingDate = DateTime.Today,
                    ShortDescription = "ShortDesc",
                    ApprenticeshipFramework = "TestFramework",
                    VacancyAddress = new AddressData
                    {
                        Latitude = 12.9m,
                        Longitude = 0.18m,
                    }
                }
            };

            var test = _vacancySummaryMapper.Map<VacancySummaryData[], IEnumerable<Domain.Entities.Vacancies.VacancySummary>>(data).ToList();

            test[0].Id.Should().Be(1);
            test[0].EmployerName.Should().Be("EmpName");
            test[0].ClosingDate.Should().Be(DateTime.Today);
            test[0].Description.Should().Be("ShortDesc");
            test[0].EmployerName.Should().Be("EmpName");
            test[0].Location.Latitude.Should().Be(12.9);
            test[0].Location.Longitude.Should().Be(0.18);
        }
    }
}
