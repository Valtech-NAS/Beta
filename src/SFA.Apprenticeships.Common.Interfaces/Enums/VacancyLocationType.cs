
namespace SFA.Apprenticeships.Common.Interfaces.Enums
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
