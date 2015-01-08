namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using AutoMapper;
    using Domain.Entities.Vacancies.Apprenticeships;
    using GatewayServiceProxy;
    using NLog;

    public class DetailApprenticeshipLevelResolver : ValueResolver<Vacancy, ApprenticeshipLevel>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                    Logger.Warn("Gateway returned vacancy (Id: {0}) with unknown apprenticeship level: {1}", source.VacancyId, source.VacancyType);
                    return ApprenticeshipLevel.Unknown;
            }
        }
    }
}
