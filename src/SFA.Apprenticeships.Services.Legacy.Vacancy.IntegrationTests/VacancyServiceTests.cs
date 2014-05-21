namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IntegrationTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration.LegacyServices;
    using SFA.Apprenticeships.Services.Common.Wcf;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
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
                    VacancyLocationType = VacancyDetailsSearchLocationType.NonNational
                }
            };

            var result = default(VacancySummaryResponse);
            var service = new WcfService<IVacancySummary>();
            service.Use(client => { result = client.Get(vacancySummaryRequest); });

            result.Should().NotBeNull();
        }
    }
}
