namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using GatewayServiceProxy;
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

                case "Deleted":
                case "ClosingDatePassed":
                case "Withdrawn":
                case "Expired":
                case "Completed":
                case "PostedInError":
                    return VacancyStatuses.Unavailable;

                default:
                    Logger.Warn("Gateway returned vacancy with unknown vacancy status: \"{1}\"", source);
                    return VacancyStatuses.Unknown;
            }
        }
    }
}
