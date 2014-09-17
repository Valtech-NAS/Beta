namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using Domain.Interfaces.Messaging;
    using Entities;

    public interface IVacancySummaryProcessor
    {
        void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage);

        void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage);
    }
}
