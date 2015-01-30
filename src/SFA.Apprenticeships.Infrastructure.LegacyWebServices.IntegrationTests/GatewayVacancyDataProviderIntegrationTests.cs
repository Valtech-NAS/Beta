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
            var container = new Container(ce =>
            {
                ce.AddRegistry<CommonRegistry>();
                ce.AddRegistry<LegacyWebServicesRegistry>();
            });

            _vacancyDataProvider = container.GetInstance<IVacancyDataProvider<ApprenticeshipVacancyDetail>>();
            _vacancyIndexDataProvider = container.GetInstance<IVacancyIndexDataProvider>();
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