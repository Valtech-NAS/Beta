namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using NLog;

    public class VacancyStatusResolver : ValueResolver<string, VacancyStatuses>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override VacancyStatuses ResolveCore(string source)
        {
            switch (source)
            {
                case "Live":
                    return VacancyStatuses.Live;

                case "Withdrawn":
                case "Deleted":
                case "Posted In Error":
                    return VacancyStatuses.Unavailable;

                case "Closed":
                case "Completed":
                    return VacancyStatuses.Expired;

                default:
                    Logger.Error("Unknown Vacancy Status received from NAS Gateway Service, defaulting to Unavailable: \"{0}\".", source);
                    return VacancyStatuses.Unavailable;
            }
        }
    }
}
