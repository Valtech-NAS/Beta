namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IntegrationTests
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Configuration;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Mapping;
    using Elastic.Common.Configuration;
    using Elastic.Common.IoC;
    using FluentAssertions;
    using IoC;
    using Mappers;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class VacancySearchTests
    {
        [Test]
        public void ShouldReturnFrameworksCount()
        {
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<VacancySearchRegistry>();
            });

            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var elasticsearchClientFactory = ObjectFactory.GetInstance<IElasticsearchClientFactory>();
            var mapper = ObjectFactory.GetInstance<IMapper>();
#pragma warning restore 0618
            
            var vacancySearchProvider = new ApprenticeshipsSearchProvider(elasticsearchClientFactory, mapper, SearchConfiguration.Instance);

            var vacancies = vacancySearchProvider.FindVacancies(new ApprenticeshipSearchParameters
            {
                ApprenticeshipLevel = "Intermediate",
                Keywords = "Chef",
                Location = new Location
                {
                    Name = "London",
                    GeoPoint = new GeoPoint
                    {
                        Latitude = 51.5072,
                        Longitude = 0.1275
                    }
                },
                PageNumber = 1,
                PageSize = 5,
                SearchRadius = 5,
                SortType = VacancySortType.ClosingDate,
                VacancyLocationType = ApprenticeshipLocationType.NonNational
            });

            vacancies.AggregationResults.Should().HaveCount(c => c > 0);
        }
    }
}