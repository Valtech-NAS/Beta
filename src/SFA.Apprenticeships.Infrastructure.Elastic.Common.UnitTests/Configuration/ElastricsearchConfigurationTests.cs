namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.UnitTests.Configuration
{
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;

    [TestFixture]
    public class ElastricsearchConfigurationTests
    {
        [Test]
        public void ShouldPopulateWithServerAndIndexDetails()
        {
            var config = ElasticsearchConfiguration.Instance;

            config.Should().NotBeNull();
            config.DefaultHost.AbsolutePath.Should().Be("http://someserver:1234");

            var index1 = config.Indexes["IndexName1"];
            index1.Should().NotBeNull();
            index1.MappingType.Should().Be(typeof (VacancySummary));

            var index2 = config.Indexes["IndexName2"];
            index2.Should().NotBeNull();
            index2.MappingType.Should().Be(typeof(VacancySummary));
        }
    }
}
