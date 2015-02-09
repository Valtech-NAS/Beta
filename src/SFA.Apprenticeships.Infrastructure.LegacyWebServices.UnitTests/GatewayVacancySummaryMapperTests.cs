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
        [Test]
        public void ShouldCreateAMap()
        {
            // Act.
            new LegacyVacancySummaryMapper().Mapper.AssertConfigurationIsValid();
        }

        [Test]
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
            var dest = new LegacyVacancySummaryMapper().Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Id.Should().Be(src.VacancyId);
            dest.ClosingDate.Should().Be(src.ClosingDate ?? DateTime.Now);
            dest.EmployerName.Should().Be(src.EmployerName);
            dest.Title.Should().Be(src.VacancyTitle);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeStandard()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeMultipleLocation()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeNational()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "National"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.National);
        }

        [Test]
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
            var dest = new LegacyVacancySummaryMapper().Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Location.Latitude.Should().Be(1.0);
            dest.Location.Longitude.Should().Be(2.0);
        }

        [Test]
        public void MapNullAddress()
        {
            // Arrange.
            var src = new GatewayServiceProxy.VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation",
                Address = null
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<GatewayServiceProxy.VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.Location.Latitude.Should().Be(0.0);
            dest.Location.Longitude.Should().Be(0.0);
        }
    }
}