namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyTypeResolver : ValueResolver<string, VacancyType>
    {
        protected override VacancyType ResolveCore(string source)
        {
            switch (source)
            {
                case "IntermediateLevelApprenticeship":
                    return VacancyType.Intermediate;
                case "AdvancedLevelApprenticeship":
                    return VacancyType.Advanced;
                default:
                    return VacancyType.Unknown;
            }
        }
    }
}
