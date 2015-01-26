namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using Application.Vacancy;
    using Application.VacancyEtl;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Common.IoC;
    using IoC;
    using StructureMap;

    [TestFixture]
    public class GatewayVacancyDataProviderIntegrationTests
    {
        [SetUp]
        public void SetUp()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });

            _vacancyDataProvider = ObjectFactory.GetInstance<IVacancyDataProvider<ApprenticeshipVacancyDetail>>();
            _vacancyIndexDataProvider = ObjectFactory.GetInstance<IVacancyIndexDataProvider>();
#pragma warning restore 0618
        }

        private IVacancyDataProvider<ApprenticeshipVacancyDetail> _vacancyDataProvider;
        private IVacancyIndexDataProvider _vacancyIndexDataProvider;

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldNotReturnVacancyDetailsForInvalidVacancyId()
        {
            var result = _vacancyDataProvider.GetVacancyDetails(123456789);

            result.Should().BeNull();
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnVacancyDetailsForValidVacancyId()
        {
            var response = _vacancyIndexDataProvider.GetVacancySummaries(1);

            var firstOrDefault = response.ApprenticeshipSummaries.FirstOrDefault();

            if (firstOrDefault == null) return;

            var firstVacancyId = firstOrDefault.Id;
            var result = _vacancyDataProvider.GetVacancyDetails(firstVacancyId);
            result.Should().NotBeNull();
        }
    }
}