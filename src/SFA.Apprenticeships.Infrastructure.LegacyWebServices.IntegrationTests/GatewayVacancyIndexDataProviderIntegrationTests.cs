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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();

                // Inject provider under test.
                x.For<IVacancyIndexDataProvider>().Use<LegacyVacancyIndexDataProvider>();
            });

            // Providers.
            _vacancyIndexDataProvider = ObjectFactory.GetInstance<IVacancyIndexDataProvider>();
#pragma warning restore 0618
        }

        [Test, Category("Integration")]
        public void ShouldReturnThePageCountForVacancies()
        {
            var result = _vacancyIndexDataProvider.GetVacancyPageCount();

            // Assert.
            result.Should().BePositive();
        }

        [Test, Category("Integration")]
        public void ShouldReturnTheFirstPageResultForVacancies()
        {
            var response = _vacancyIndexDataProvider.GetVacancySummaries(1);

            response.Should().NotBeNull();
        }

    }
}