namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using GatewayServiceProxy;
    using NLog;

    public class VacancyStatusResolver : ValueResolver<Vacancy, VacancyStatuses>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override VacancyStatuses ResolveCore(Vacancy source)
        {
            switch (source.Status)
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
                    Logger.Warn("Gateway returned vacancy (Id: {0}) with unknown vacancy status: \"{1}\"", source.VacancyId, source.Status);
                    return VacancyStatuses.Unknown;
            }
        }
    }
}
