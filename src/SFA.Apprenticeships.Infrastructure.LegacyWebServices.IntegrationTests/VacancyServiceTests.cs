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
        [TestCase, Category("Integration")]
        public void ShouldReturnMappedCollectionFromGetVacancySummary()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var service = ObjectFactory.GetInstance<IVacancyIndexDataProvider>();
#pragma warning restore 0618

            List<VacancySummary> result = service.GetVacancySummaries(1).ToList();

            result.Should().NotBeNullOrEmpty();
            result.ForEach(r => r.VacancyType.Should().Be(VacancyType.Apprenticeship));
        }
    }
}