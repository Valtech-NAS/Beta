namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.Consumers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Entities;
    using EasyNetQ.AutoSubscribe;

    public class ApplicationStatusSummaryPageConsumerAsync : IConsumeAsync<ApplicationUpdatePage>
    {
        // http://www.mariuszwojcik.com/2014/05/10/how-to-allow-easynetq-finish-processing-message-on-application-stop/
        private readonly IApplicationStatusProcessor _applicationStatusProcessor;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _applicationStatusSummaryPageConsumerResetEvent = new ManualResetEvent(true);

        private CancellationToken CancellationToken { get { return _cancellationTokenSource.Token; } }

        public ApplicationStatusSummaryPageConsumerAsync(IApplicationStatusProcessor applicationStatusProcessor)
        {
            _applicationStatusProcessor = applicationStatusProcessor;
        }

        [AutoSubscriberConsumer(SubscriptionId = "ApplicationStatusSummaryPageConsumerAsync")]
        public Task Consume(ApplicationUpdatePage message)
        {
            return Task.Run(() =>
            {
                if (CancellationToken.IsCancellationRequested)
                {
                    _applicationStatusSummaryPageConsumerResetEvent.Set();
                    Thread.Sleep(Timeout.Infinite);
                }

                _applicationStatusSummaryPageConsumerResetEvent.Reset();

                try
                {
                    _applicationStatusProcessor.QueueApplicationStatuses(message);
                }
                finally
                {
                    _applicationStatusSummaryPageConsumerResetEvent.Set();
                }

            });
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();


            // disposing consumer
            if (_applicationStatusSummaryPageConsumerResetEvent.WaitOne(TimeSpan.FromSeconds(15)))
            {
                // Consumer hasn't finished on time and was forcefully disposed. 
                //The message was probably NACKed
            }
            else
            {
                _applicationStatusSummaryPageConsumerResetEvent.Reset();
                // Waiting for message to be ACKed.
                _applicationStatusSummaryPageConsumerResetEvent.WaitOne(TimeSpan.FromSeconds(5));
            }
            // Consumer automatically disposed
            // All consumers stopped
        }
    }
}
