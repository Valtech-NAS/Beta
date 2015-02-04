using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.UnitTests
{
    using System;
    using FluentAssertions;
    using Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class GatewayVacancySummaryMapperTests
    {
        [SetUp]
        public void Setup()
        {
            _mapper = new LegacyVacancySummaryMapper();
        }

        private LegacyVacancySummaryMapper _mapper;

        [TestCase]
        public void ShouldCreateAMap()
        {
            // Act.
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [TestCase]
        public void ShouldMapAllOneToOneFields()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyId = 42,
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                ClosingDate = DateTime.Today.AddDays(1),
                EmployerName = "EmployerName",
                VacancyTitle = "VacancyTitle",
            };

            // Act.
            var dest = _mapper.Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Id.Should().Be(src.VacancyId);
            dest.ClosingDate.Should().Be(src.ClosingDate ?? DateTime.Now);
            dest.EmployerName.Should().Be(src.EmployerName);
            dest.Title.Should().Be(src.VacancyTitle);
        }

        [TestCase]
        public void ShouldMapVacancyLocationTypeStandard()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard"
            };

            // Act.
            var dest = _mapper.Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [TestCase]
        public void ShouldMapVacancyLocationTypeMultipleLocation()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation"
            };

            // Act.
            var dest = _mapper.Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [TestCase]
        public void ShouldMapVacancyLocationTypeNational()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "National"
            };

            // Act.
            var dest = _mapper.Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.National);
        }

        [TestCase]
        public void ShouldMapVacancyLocationWhenSpecified()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                Address = new GatewayServiceProxy.VacancySummaryAddress
                {
                    Latitude = 1.0m,
                    Longitude = 2.0m
                }
            };

            // Act.
            var dest = _mapper.Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Location.Latitude.Should().Be(1.0);
            dest.Location.Longitude.Should().Be(2.0);
        }

        [TestCase]
        public void ShouldThrowIfUnknownVacancyLocationType()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation",
                Address = null
            };

            // Act.
            var dest = _mapper.Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.Location.Latitude.Should().Be(0.0);
            dest.Location.Longitude.Should().Be(0.0);
        }
    }
}