namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyTypeResolver : ValueResolver<string, VacancyType>
    {
        protected override VacancyType ResolveCore(string source)
        {
            switch (source)
            {
                case "IntermediateLevelApprenticeship":
                case "AdvancedLevelApprenticeship":
                case "HigherApprenticeship":
                    return VacancyType.Apprenticeship;
                case "Traineeship":
                    return VacancyType.Traineeship;
                default:
                    return VacancyType.Unknown;
            }
        }
    }
}