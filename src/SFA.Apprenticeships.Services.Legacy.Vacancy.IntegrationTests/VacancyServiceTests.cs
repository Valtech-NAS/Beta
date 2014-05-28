namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IntegrationTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Interfaces.Enums;
    using SFA.Apprenticeships.Domain.Interfaces.Services;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;
    using StructureMap;

    [TestFixture]
    public class VacancyServiceTests
    {
        private IVacancySummaryService _service;
        private ILegacyServicesConfiguration _legacyServicesConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {
            Apprenticeships.Common.IoC.IoC.Initialize();
            _legacyServicesConfiguration = ObjectFactory.GetInstance<ILegacyServicesConfiguration>();
            _service = ObjectFactory.GetInstance<IVacancySummaryService>();
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
            var service = ObjectFactory.GetInstance<IVacancySummaryService>();
            var result = service.GetVacancySummary(VacancyLocationType.NonNational, 1);

            result.ToList().Should().NotBeNullOrEmpty();
        }
    }
}
