using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.Apprenticeships.Common.Entities.Vacancy;
using SFA.Apprenticeships.Common.Interfaces.Enums;
using SFA.Apprenticeships.Common.Interfaces.ReferenceData;
using SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers;
using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;

namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IntegrationTests
{
    [TestFixture]
    public class MapperTests
    {
        private Mock<IReferenceDataService> _service;
        private VacancySummaryMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new Mock<IReferenceDataService>();
            _mapper = new VacancySummaryMapper(_service.Object);
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
                VacancyLocationType = VacancyLocationType.NonNational.ToString(),
                VacancyType = VacancyType.Intermediate.ToString(),
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
    }
}
