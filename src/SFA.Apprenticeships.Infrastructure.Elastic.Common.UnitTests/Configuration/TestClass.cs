namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.UnitTests.Configuration
{
    using Nest;

    [ElasticType(Name = "test_mapping_class")]
    public class TestMappingClass
    {
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public long Id { get; set; }

        [ElasticProperty(Index = FieldIndexOption.analyzed)]
        public string Dummy { get; set; }
    }
}
