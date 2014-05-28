
namespace SFA.Apprenticeships.Domain.Entities.Vacancy
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
