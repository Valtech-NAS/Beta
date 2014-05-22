﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers;
using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;

namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IntegrationTests
{
    using SFA.Apprenticeships.Common.Helpers;

    [TestFixture]
    public class MapperTests
    {
        private VacancySummaryMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new VacancySummaryMapper();
            _mapper.Initialize();
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
            test.Location.lat.Should().Be(12.9);
            test.Location.lon.Should().Be(0.18);
        }

        [TestCase]
        public void ShouldReturnEnumsFromEnumResolver()
        {
            var data = new VacancySummaryData
            {
                VacancyReference = 1,
                VacancyLocationType = VacancyLocationType.NonNational.GetDescription(),
                VacancyType = VacancyType.Intermediate.GetDescription(),
            };

            var test = _mapper.Map<VacancySummaryData, VacancySummary>(data);

            test.Id.Should().Be(1);
            test.TypeOfLocation.Should().Be(VacancyLocationType.NonNational);
            test.TypeOfVacancy.Should().Be(VacancyType.Intermediate);
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
