namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using Application.Vacancy;
    using Application.VacancyEtl;
    using Common.IoC;
    using Domain.Entities.Vacancies;
    using IoC;
    using NUnit.Framework;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using StructureMap;
    using VacancyDetail;
    using VacancySummary;
    using FluentAssertions;

    [TestFixture]
    public class GatewayVacancyIndexDataProviderIntegrationTests
    {
        private IVacancyIndexDataProvider _vacancyIndexDataProvider;
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<GatewayWebServicesRegistry>();

                // Inject provider under test.
                x.For<IVacancyIndexDataProvider>().Use<GatewayVacancyIndexDataProvider>();
            });

            // Providers.
            _vacancyIndexDataProvider = ObjectFactory.GetInstance<IVacancyIndexDataProvider>();
        }

        [Test]
        public void ShouldReturnThePageCountForVacancies()
        {
            var result = _vacancyIndexDataProvider.GetVacancyPageCount(VacancyLocationType.National);

            // Assert.
            result.Should().BePositive();
        }

        [Test]
        public void ShouldReturnTheFirstPageResultForVacancies()
        {
            var response = _vacancyIndexDataProvider.GetVacancySummaries(VacancyLocationType.NonNational, 1);

            response.Should().NotBeNull();
        }

    }
}