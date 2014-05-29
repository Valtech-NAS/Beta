
namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities
{
    using System.ComponentModel;

    public enum VacancyType
    {
        Unknown = 0,

        [Description("IntermediateLevelApprenticeship")] 
        Intermediate,

        [Description("AdvancedLevelApprenticeship")] 
        Advanced,
    }
}
