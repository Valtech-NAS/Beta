namespace SFA.Apprenticeships.Application.ApplicationUpdate
{
    using System;
    using Domain.Entities.Vacancies;
    using Entities;

    public interface IApplicationStatusProcessor
    {
        void QueueApplicationStatusesPages();

        void QueueApplicationStatuses(ApplicationUpdatePage applicationStatusSummaryPage);

        void ProcessApplicationStatuses(ApplicationStatusSummary applicationStatusSummary);

        void ProcessApplicationStatuses(VacancyStatusSummary vacancyStatusSummary);
    }
}
