
namespace SFA.Apprenticeships.Common.Interfaces.Enums
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
