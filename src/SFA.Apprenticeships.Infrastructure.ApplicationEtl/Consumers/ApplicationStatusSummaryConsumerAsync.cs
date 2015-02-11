namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Entities;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationStatusSummaryConsumerAsync : IConsumeAsync<ApplicationStatusSummary>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _applicationStatusSummaryConsumerResetEvent = new ManualResetEvent(true);
        private readonly IMessageBus _bus;

        private CancellationToken CancellationToken { get { return _cancellationTokenSource.Token; } }

        public ApplicationStatusSummaryConsumerAsync(IApplicationStatusProcessor applicationStatusProcessor, IMessageBus bus)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
            _bus = bus;
        }

        [SubscriptionConfiguration(PrefetchCount = 20)]
        [AutoSubscriberConsumer(SubscriptionId = "ApplicationStatusSummaryConsumerAsync")]
        public Task Consume(ApplicationStatusSummary applicationStatusSummaryToProcess)
        {
            return Task.Run(() =>
            {
                if (CancellationToken.IsCancellationRequested)
                {
                    _applicationStatusSummaryConsumerResetEvent.Set();
                    Thread.Sleep(Timeout.Infinite);
                }

                _applicationStatusSummaryConsumerResetEvent.Reset();

                try
                {
                    _applicationStatusProcessor.ProcessApplicationStatuses(applicationStatusSummaryToProcess);

                    // determine whether this message is from an already propagated vacancy status summary
                    var isReprocessing = applicationStatusSummaryToProcess.ApplicationId != Guid.Empty;

                    if (!isReprocessing)
                    {
                        var vacancyStatusSummary = new VacancyStatusSummary
                        {
                            LegacyVacancyId = applicationStatusSummaryToProcess.LegacyVacancyId,
                            VacancyStatus = applicationStatusSummaryToProcess.VacancyStatus,
                            ClosingDate = applicationStatusSummaryToProcess.ClosingDate
                        };

                        _bus.PublishMessage(vacancyStatusSummary);
                    }
                }
                finally
                {
                    _applicationStatusSummaryConsumerResetEvent.Set();
                }
            });
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();


            // disposing consumer
            if (_applicationStatusSummaryConsumerResetEvent.WaitOne(TimeSpan.FromSeconds(15)))
            {
                // Consumer hasn't finished on time and was forcefully disposed. 
                //The message was probably NACKed
            }
            else
            {
                _applicationStatusSummaryConsumerResetEvent.Reset();
                // Waiting for message to be ACKed.
                _applicationStatusSummaryConsumerResetEvent.WaitOne(TimeSpan.FromSeconds(5));
            }
            // Consumer automatically disposed
            // All consumers stopped
        }
    }
}
