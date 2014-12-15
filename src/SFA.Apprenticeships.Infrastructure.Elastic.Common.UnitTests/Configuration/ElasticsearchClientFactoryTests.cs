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

            clientFactory.GetIndexNameForType(typeof(ApprenticeshipSummary)).Should().Be("ApprenticeshipIndex");
            clientFactory.GetIndexNameForType(typeof(TraineeshipSummary)).Should().Be("TraineeshipIndex");
            clientFactory.GetIndexNameForType(typeof(TestMappingClass)).Should().Be("TestMappingClassIndex");
        }

        [Test]
        public void ShouldReturnDocumentTypesFromConfigurationForMappedObjectType()
        {
            var clientFactory = new ElasticsearchClientFactory(ElasticsearchConfiguration.Instance, false);

            clientFactory.GetDocumentNameForType(typeof(ApprenticeshipSummary)).Should().Be("apprenticeships");
            clientFactory.GetDocumentNameForType(typeof(TraineeshipSummary)).Should().Be("trainseeships");
            clientFactory.GetDocumentNameForType(typeof(TestMappingClass)).Should().Be("test_mapping_class");
        }
    }
}
