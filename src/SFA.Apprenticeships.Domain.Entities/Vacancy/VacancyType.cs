
namespace SFA.Apprenticeships.Domain.Entities.Vacancy
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
