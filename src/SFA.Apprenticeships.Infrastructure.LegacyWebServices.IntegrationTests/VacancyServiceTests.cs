namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using Application.VacancyEtl;
    using Caching.Memory.IoC;
    using Common.IoC;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class VacancyServiceTests
    {
        [TestCase, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnMappedCollectionFromGetVacancySummary()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<MemoryCacheRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
            });

            var service = container.GetInstance<IVacancyIndexDataProvider>();

            var result = service.GetVacancySummaries(1);

            result.ApprenticeshipSummaries.Should().NotBeNullOrEmpty();
            result.TraineeshipSummaries.Should().NotBeNullOrEmpty();
        }
    }
}