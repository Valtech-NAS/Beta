using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;

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
