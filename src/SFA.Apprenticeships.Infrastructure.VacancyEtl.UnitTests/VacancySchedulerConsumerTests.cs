namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Messaging;
    using Elastic.Common.Entities;
    using Moq;
    using NUnit.Framework;
    using Application.VacancyEtl;
    using Application.VacancyEtl.Entities;
    using Consumers;
    using VacancyIndexer;

    [TestFixture]
    public class VacancySchedulerConsumerTests
    {
        private Mock<ILogService> _logService;
        private Mock<IProcessControlQueue<StorageQueueMessage>> _messageServiceMock;
        private Mock<IVacancySummaryProcessor> _vacancySummaryProcessorMock;
        private Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>> _apprenticeshipIndexerService;
        private Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>> _traineeshipsIndexerService;

        [SetUp]
        public void SetUp()
        {
            _logService = new Mock<ILogService>();
            _messageServiceMock = new Mock<IProcessControlQueue<StorageQueueMessage>>();
            _vacancySummaryProcessorMock = new Mock<IVacancySummaryProcessor>();
            _apprenticeshipIndexerService = new Mock<IVacancyIndexerService<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>>();
            _traineeshipsIndexerService = new Mock<IVacancyIndexerService<TraineeshipSummaryUpdate, TraineeshipSummary>>();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldKeepPoppingAndRemovingScheduleMessagesFromQueueUntilLastMessage(int queuedScheduledMessages)
        {
            var scheduledMessageQueue = GetScheduledMessagesQueue(queuedScheduledMessages);
            _messageServiceMock.Setup(x => x.GetMessage()).Returns(scheduledMessageQueue.Dequeue);
            var vacancyConsumer = new VacancyEtlControlQueueConsumer(_messageServiceMock.Object, _vacancySummaryProcessorMock.Object, _apprenticeshipIndexerService.Object, _traineeshipsIndexerService.Object, _logService.Object);
            var task = vacancyConsumer.CheckScheduleQueue();
            task.Wait();

            _messageServiceMock.Verify(x => x.GetMessage(), Times.Exactly(queuedScheduledMessages + 1));
            _messageServiceMock.Verify(x => x.DeleteMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(queuedScheduledMessages == 0 ? 0 : queuedScheduledMessages - 1));
            _apprenticeshipIndexerService.Verify(x => x.CreateScheduledIndex(It.Is<DateTime>(d => d == DateTime.Today)), Times.Exactly(queuedScheduledMessages > 0 ? 1 : 0));
            _vacancySummaryProcessorMock.Verify(x => x.QueueVacancyPages(It.IsAny<StorageQueueMessage>()), Times.Exactly(queuedScheduledMessages == 0 ? 0 : 1));
        }

        private Queue<StorageQueueMessage> GetScheduledMessagesQueue(int count)
        {
            var queue = new Queue<StorageQueueMessage>();

            for (int i = count; i > 0; i--)
            {
                var storageScheduleMessage = new StorageQueueMessage
                {
                    ClientRequestId = Guid.NewGuid(),
                    ExpectedExecutionTime = DateTime.Today,
                    SchedulerJobId = i.ToString(CultureInfo.InvariantCulture)
                };

                queue.Enqueue(storageScheduleMessage);
            }

            queue.Enqueue(null);
            return queue;
        }
    }
}
