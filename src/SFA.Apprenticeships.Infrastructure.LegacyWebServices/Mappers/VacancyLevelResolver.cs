namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyLevelResolver : ValueResolver<string, VacancyLevel>
    {
        protected override VacancyLevel ResolveCore(string source)
        {
            switch (source)
            {
                case "IntermediateLevelApprenticeship":
                    return VacancyLevel.Intermediate;

                case "AdvancedLevelApprenticeship":
                    return VacancyLevel.Advanced;

                default:
                    return VacancyLevel.Unknown;
            }
        }
    }
}
