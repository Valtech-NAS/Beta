namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.UnitTests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using Common.Configuration;
    using Entities;

    [TestFixture]
    public class ElasticsearchClientFactoryTests
    {
        [Test]
        public void ShouldReturnIndexNamesFromConfigurationForMappedObjectType()
        {
            var clientFactory = new ElasticsearchClientFactory(ElasticsearchConfiguration.Instance, false);
            
            clientFactory.GetIndexNameForType(typeof(VacancySummary)).Should().Be("VacancySummaryIndex");
            clientFactory.GetIndexNameForType(typeof(TestMappingClass)).Should().Be("TestMappingClassIndex");
        }

        [Test]
        public void ShouldReturnDocumentTypesFromConfigurationForMappedObjectType()
        {
            var clientFactory = new ElasticsearchClientFactory(ElasticsearchConfiguration.Instance, false);

            clientFactory.GetDocumentNameForType(typeof(VacancySummary)).Should().Be("vacancy");
            clientFactory.GetDocumentNameForType(typeof(TestMappingClass)).Should().Be("test_mapping_class");
        }
    }
}
