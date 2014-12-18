namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class VacancyStatusResolver : ValueResolver<string, VacancyStatuses>
    {
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
                    return VacancyStatuses.Unknown;
            }
        }
    }
}
