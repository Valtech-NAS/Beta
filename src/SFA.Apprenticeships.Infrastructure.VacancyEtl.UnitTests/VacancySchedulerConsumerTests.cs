namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Application.Interfaces.Messaging;
    using SFA.Apprenticeships.Application.VacancyEtl;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.VacancyEtl.Consumers;

    [TestFixture]
    public class VacancySchedulerConsumerTests
    {
        private Mock<IProcessControlQueue<StorageQueueMessage>> _messageServiceMock;
        private Mock<IVacancySummaryProcessor> _vacancySummaryProcessorMock;

        [SetUp]
        public void SetUp()
        {
            _messageServiceMock = new Mock<IProcessControlQueue<StorageQueueMessage>>();
            _vacancySummaryProcessorMock = new Mock<IVacancySummaryProcessor>();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldKeepPoppingAndRemovingScheduleMessagesFromQueueUntilLastMessage(int queuedScheduledMessages)
        {
            var scheduledMessageQueue = GetScheduledMessagesQueue(queuedScheduledMessages);
            _messageServiceMock.Setup(x => x.GetMessage()).Returns(scheduledMessageQueue.Dequeue);
            var vacancyConsumer = new VacancySchedulerConsumer(_messageServiceMock.Object, _vacancySummaryProcessorMock.Object);
            var task = vacancyConsumer.CheckScheduleQueue();
            task.Wait();

            _messageServiceMock.Verify(x => x.GetMessage(), Times.Exactly(queuedScheduledMessages + 1));
            _messageServiceMock.Verify(x => x.DeleteMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(queuedScheduledMessages == 0 ? 0 : queuedScheduledMessages - 1));
            _vacancySummaryProcessorMock.Verify(x => x.QueueVacancyPages(It.IsAny<StorageQueueMessage>()), Times.Exactly(queuedScheduledMessages == 0 ? 0 : 1));
        }

        private Queue<StorageQueueMessage> GetScheduledMessagesQueue(int count)
        {
            var queue = new Queue<StorageQueueMessage>();

            for (int i = count; i > 0; i--)
            {
                var storageScheduleMessage = new StorageQueueMessage
                {
                    ClientRequestId = Guid.NewGuid().ToString(),
                    SchedulerJobId = i.ToString()
                };

                queue.Enqueue(storageScheduleMessage);
            }

            queue.Enqueue(null);
            return queue;
        }
    }
}
