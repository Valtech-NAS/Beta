namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.UnitTests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;

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
    }
}
