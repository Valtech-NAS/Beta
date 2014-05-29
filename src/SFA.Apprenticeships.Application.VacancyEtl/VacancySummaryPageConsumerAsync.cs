namespace SFA.Apprenticeships.Application.VacancyEtl
{
    using System;
    using System.Threading.Tasks;

    public class VacancySummaryPageConsumerAsync : IConsumeAsync<VacancySummaryPage>
    {
        private readonly IVacancySummaryService _service;
        private readonly IBus _bus;

        public VacancySummaryPageConsumerAsync(IBus bus, IVacancySummaryService service)
        {
            _bus = bus;
            _service = service;
        }

        [AutoSubscriberConsumer(SubscriptionId = "VacancySummaryPageConsumerAsync")]
        public Task Consume(VacancySummaryPage message)
        {
            return Task.Run(() => ConsumeTask(message));
        }

        private void ConsumeTask(VacancySummaryPage message)
        {
            try
            {
                var vacancies = _service.GetVacancySummary(message.VacancyLocation, message.PageNumber).ToList();
                vacancies.ForEach(x => x.UpdateReference = message.UpdateReference);

                Parallel.ForEach(
                    vacancies,
                    new ParallelOptions() {MaxDegreeOfParallelism = 5},
                    vacancy => _bus.Publish(vacancy));
            }
            catch (Exception ex)
            {
                // TODO::High::Log this error
            }
        }
    }
}
