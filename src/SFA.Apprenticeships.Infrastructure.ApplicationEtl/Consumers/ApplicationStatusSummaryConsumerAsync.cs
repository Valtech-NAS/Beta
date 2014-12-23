namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Domain.Entities.Applications;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationStatusSummaryConsumerAsync : IConsumeAsync<ApplicationStatusSummary>
    {
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _applicationStatusSummaryConsumerResetEvent = new ManualResetEvent(true);

        private CancellationToken CancellationToken { get { return _cancellationTokenSource.Token; } }

        public ApplicationStatusSummaryConsumerAsync(IApplicationStatusProcessor applicationStatusProcessor)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
        }

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
