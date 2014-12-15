namespace SFA.Apprenticeships.Application.VacancyEtl.Entities
{
    using System;

    public interface IVacancyUpdate
    {
        DateTime ScheduledRefreshDateTime { get; set; }
    }
}
