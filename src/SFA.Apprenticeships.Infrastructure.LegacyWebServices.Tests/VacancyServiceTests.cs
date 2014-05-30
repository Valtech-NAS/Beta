namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Common.Wcf;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummaryProxy;
    using StructureMap;

    [TestFixture]
    public class VacancyServiceTests
    {
        private IVacancyService _service;
        private ILegacyServicesConfiguration _legacyServicesConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {

            _legacyServicesConfiguration = ObjectFactory.GetInstance<ILegacyServicesConfiguration>();
            _service = ObjectFactory.GetInstance<IVacancyService>();
        }

        [TestCase]
        public void TheServiceEndpointShouldRespond()
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData()
                {
                    PageIndex = 1,
                    VacancyLocationType = VacancyDetailsSearchLocationType.NonNational.ToString()
                }
            };

            var result = default(VacancySummaryResponse);
            var service = ObjectFactory.GetInstance<IWcfService<IVacancySummary>>();
            service.Use(client => { result = client.Get(vacancySummaryRequest); });

            result.Should().NotBeNull();
        }

        [TestCase]
        public void ShouldReturnMappedCollectionFromGetVacancySummary()
        {
            var service = ObjectFactory.GetInstance<IVacancyService>();
            var result = service.GetVacancySummary(VacancyLocationType.NonNational, 1);

            result.ToList().Should().NotBeNullOrEmpty();
        }
    }
}
