namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using Domain.Interfaces.Messaging;
    using Entities;
    using SFA.Apprenticeships.Domain.Entities.Vacancies;

    public interface IVacancySummaryProcessor
    {
        void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage);

        void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage);

        void QueueVacancyIfExpired(VacancySummary vacancySummary);
    }
}
