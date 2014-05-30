namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;

    public interface IVacancySummaryProcessor
    {
        void QueueVacancyPages(StorageQueueMessage scheduledQueueMessage);
        void QueueVacancySummaries(VacancySummaryPage vacancySummaryPage);
    }
}
