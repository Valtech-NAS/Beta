namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.UnitTests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using Common.Configuration;
    using Entities;

    [TestFixture]
    public class ElastricsearchConfigurationTests
    {
        [Test]
        public void ShouldPopulateWithServerAndIndexDetails()
        {
            var config = ElasticsearchConfiguration.Instance;

            config.Should().NotBeNull();
            config.DefaultHost.OriginalString.Should().Be("http://someserver:1234");
            config.NodeCount = 1;
            config.Timeout = 30;

            var index1 = config.Indexes["TestMappingClassIndex"];
            index1.Should().NotBeNull();
            index1.MappingType.Should().Be(typeof(TestMappingClass));

            var index2 = config.Indexes["ApprenticeshipIndex"];
            index2.Should().NotBeNull();
            index2.MappingType.Should().Be(typeof(ApprenticeshipSummary));

            var index3 = config.Indexes["TraineeshipIndex"];
            index3.Should().NotBeNull();
            index3.MappingType.Should().Be(typeof(TraineeshipSummary));
        }
    }
}
