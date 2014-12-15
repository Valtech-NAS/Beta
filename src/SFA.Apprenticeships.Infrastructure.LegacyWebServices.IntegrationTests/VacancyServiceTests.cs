using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.VacancyEtl;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class VacancyServiceTests
    {
        [TestCase, Category("Integration"), Category("SmokeTests")]
        public void ShouldReturnMappedCollectionFromGetVacancySummary()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<IVacancyIndexDataProvider>();
#pragma warning restore 0618

            var result = service.GetVacancySummaries(1);

            result.ApprenticeshipSummaries.Should().NotBeNullOrEmpty();
            result.TraineeshipSummaries.Should().NotBeNullOrEmpty();
        }
    }
}