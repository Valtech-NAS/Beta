
namespace SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities
{
    using System.ComponentModel;

    public enum VacancyLocationType
    {
        Unknown = 0,

        [Description("NonNational")]
        NonNational,

        [Description("National")] 
        National,
    }
}
