namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
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
                case "Live": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return VacancyStatuses.Live;
                case "Withdrawn": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return VacancyStatuses.Withdrawn;
                case "Expired": //todo: confirm value from w/s - awaiting response from John @ Capgemini
                    return VacancyStatuses.Expired;
                default:
                    return VacancyStatuses.Unknown;
            }
        }
    }
}
