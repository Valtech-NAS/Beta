namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using Application.Vacancy;
    using Common.IoC;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using Repositories.Applications.IoC;
    using Repositories.Candidates.IoC;
    using StructureMap;
    using VacancyDetail;

    // TODO: AG: US484: use or remove this integration test.

    [TestFixture]
    public class GatewayVacancyDataProviderIntegrationTests
    {
        private IVacancyDataProvider _provider;

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();

                // Inject provider under test.
                x.For<IVacancyDataProvider>().Use<GatewayVacancyDataProvider>();
            });

            // Providers.
            _provider = ObjectFactory.GetInstance<IVacancyDataProvider>();
        }

        [Test, Category("Integration")]
        [Ignore]
        public void ShouldNotReturnVacancyDetailsForNonExistentVacancyId()
        {
            // Arrange.

            // Act.
            var result = _provider.GetVacancyDetails(-1);

            // Assert.
            result.Should().BeNull();
        }
    }
}
