namespace SFA.Apprenticeships.Services.VacancyEtl.Consumers
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.VacancyEtl.Entities;

    public class VacancySummaryPageConsumerAsync : IConsumeAsync<VacancySummaryPage>
    {
        private IVacancySummaryService _service;
        private IBus _bus;

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
