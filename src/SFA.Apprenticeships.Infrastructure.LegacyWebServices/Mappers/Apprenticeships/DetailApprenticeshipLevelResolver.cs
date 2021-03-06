﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies.Apprenticeships;
    using GatewayServiceProxy;

    public class DetailApprenticeshipLevelResolver : ValueResolver<Vacancy, ApprenticeshipLevel>
    {
        protected override ApprenticeshipLevel ResolveCore(Vacancy source)
        {
            switch (source.VacancyType)
            {
                case "IntermediateLevelApprenticeship":
                    return ApprenticeshipLevel.Intermediate;
                case "AdvancedLevelApprenticeship":
                    return ApprenticeshipLevel.Advanced;
                case "HigherApprenticeship":
                    return ApprenticeshipLevel.Higher;
                default:
                    throw new ArgumentOutOfRangeException("source", "Unknown Apprenticeship Level received from NAS Gateway Service: " + source.VacancyType);
            }
        }
    }
}
