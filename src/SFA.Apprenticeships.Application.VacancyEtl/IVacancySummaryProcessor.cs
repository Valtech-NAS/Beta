namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using Entities;

    public interface IVacancySummaryProcessor
    {
        void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage);
        void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage);
    }
}
