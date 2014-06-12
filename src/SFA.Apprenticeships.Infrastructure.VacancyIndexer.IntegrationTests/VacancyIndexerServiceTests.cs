namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.IntegrationTests
{
    using System.Linq;
    using FluentAssertions;
    using Nest;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;
    using SFA.Apprenticeships.Infrastructure.VacancyIndexer.Services;
    using StructureMap;

    [TestFixture]
    public class VacancyIndexerServiceTests
    {
        private ElasticClient _elasticClient;
        private readonly ElasticsearchConfiguration _elasticsearchConfiguration = ElasticsearchConfiguration.Instance;

        [SetUp]
        public void SetUp()
        {
            var settings = new ConnectionSettings(_elasticsearchConfiguration.DefaultHost);
            _elasticClient = new ElasticClient(settings);
        }

        [Test]
        public void ShouldGetItemCountWithUpdateReference()
        {
            var vacancyIndexer = ObjectFactory.GetInstance<IVacancyIndexerService>();
            var elasticsearchClientFactory = ObjectFactory.GetInstance<IElasticsearchClientFactory>();
            var index = elasticsearchClientFactory.GetIndexNameForType(typeof (VacancySummary));
            var documentTypeName = elasticsearchClientFactory.GetDocumentNameForType(typeof(Elastic.Common.Entities.VacancySummary));

            var items = _elasticClient.Search<VacancySummary>(d =>
            {
                d.Index("vacancies_search");
                d.Type(documentTypeName);
                d.From(0);
                d.Take(10);
                d.MatchAll();
                return d;
            });

            items.Should().NotBeNull();
            items.Total.Should().BeGreaterThan(0);

            var itemReference = items.Documents.First().UpdateReference;

            //var count = vacancyIndexer.VacanciesWithoutUpdateReference(itemReference);
        }
    }
}
