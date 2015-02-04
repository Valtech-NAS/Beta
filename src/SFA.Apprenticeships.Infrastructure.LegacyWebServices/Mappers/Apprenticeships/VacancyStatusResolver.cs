namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using System;
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

                case "Withdrawn":
                case "Deleted":
                case "Posted In Error":
                case "PostedInError":
                    return VacancyStatuses.Unavailable;

                case "Closed":
                case "ClosingDatePassed":
                case "Completed":
                    return VacancyStatuses.Expired;

                default:
                    throw new ArgumentOutOfRangeException("source",
                        string.Format("Unknown Vacancy Status received from NAS Gateway Service: \"{0}\"", source));
            }
        }
    }
}
