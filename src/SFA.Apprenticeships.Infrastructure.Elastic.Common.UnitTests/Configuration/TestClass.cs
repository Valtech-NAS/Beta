namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.UnitTests.Configuration
{
    using Nest;

    [ElasticType(Name = "test_mapping_class")]
    public class TestMappingClass
    {
        [ElasticProperty(Index = FieldIndexOption.NotAnalyzed)]
        public long Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.Analyzed)]
        public string Dummy { get; set; }
    }
}
