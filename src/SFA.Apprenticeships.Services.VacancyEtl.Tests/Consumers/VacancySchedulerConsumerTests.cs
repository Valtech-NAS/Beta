namespace SFA.Apprenticeships.Services.VacancyEtl.Tests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using EasyNetQ;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Moq;
    using NUnit.Framework;
    using SFA.Apprenticeships.Domain.Interfaces.Enums;
    using SFA.Apprenticeships.Services.VacancyEtl.Consumers;
    using SFA.Apprenticeships.Services.VacancyEtl.Entities;

    [TestFixture]
    public class VacancySchedulerConsumerTests
    {
        private Mock<IBus> _busMock;
        private Mock<IAzureCloudClient> _cloudClientMock;
        private Mock<IVacancySummaryService> _vacancySummaryServiceMock;

        [SetUp]
        public void SetUp()
        {
            _busMock = new Mock<IBus>();
            _cloudClientMock = new Mock<IAzureCloudClient>();
            _vacancySummaryServiceMock = new Mock<IVacancySummaryService>();
        }

        [TestCase]
        public void ShouldNotCheckVanaciesIfNoScheduleEventOnQueue()
        {
            _cloudClientMock.Setup(x => x.GetMessage(It.IsAny<string>())).Returns(default(CloudQueueMessage));
            var vacancyConsumer = new VacancySchedulerConsumer(_busMock.Object, _cloudClientMock.Object, _vacancySummaryServiceMock.Object);
            var task = vacancyConsumer.CheckScheduleQueue();
            task.Wait();

            _cloudClientMock.Verify(x => x.GetMessage(It.Is<string>(s => s == "vacancysearchdatacontrol")), Times.Once);
            _vacancySummaryServiceMock.Verify(x => x.GetVacancyPageCount(It.IsAny<VacancyLocationType>()), Times.Never);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(1, 1, 0)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 4, 7)]
        [TestCase(4, 10, 5)]
        public void ShouldCallServicesRightNumberOfTimesBasedOnResponses(int queueItems, int nationalVacancyPages, int nonNationalVancanyPages)
        {

            var azureQueueMessages = GetAzureScheduledMessagesQueue(queueItems);
            _cloudClientMock.Setup(x => x.GetMessage(It.IsAny<string>())).Returns(azureQueueMessages.Dequeue);
            _vacancySummaryServiceMock.Setup(x => x.GetVacancyPageCount(VacancyLocationType.National)).Returns(nationalVacancyPages);
            _vacancySummaryServiceMock.Setup(x => x.GetVacancyPageCount(VacancyLocationType.NonNational)).Returns(nonNationalVancanyPages);

            var vacancyConsumer = new VacancySchedulerConsumer(_busMock.Object, _cloudClientMock.Object, _vacancySummaryServiceMock.Object);
            var task = vacancyConsumer.CheckScheduleQueue();
            task.Wait();

            _cloudClientMock.Verify(x => x.GetMessage(It.Is<string>(s => s == "vacancysearchdatacontrol")), Times.Exactly(queueItems + 1));
            _cloudClientMock.Verify(x => x.DeleteMessage(It.Is<string>(s => s == "vacancysearchdatacontrol"), It.IsAny<CloudQueueMessage>()), Times.Exactly(queueItems));
            _vacancySummaryServiceMock.Verify(x => x.GetVacancyPageCount(It.IsAny<VacancyLocationType>()), Times.Exactly(queueItems == 0 ? 0 : 2));
            _busMock.Verify(x => x.Publish(It.IsAny<VacancySummaryPage>()), Times.Exactly(nationalVacancyPages + nonNationalVancanyPages));
        }

        private Queue<CloudQueueMessage> GetAzureScheduledMessagesQueue(int count)
        {
            var queue = new Queue<CloudQueueMessage>();
            var serializer = new XmlSerializer(typeof(StorageQueueMessage));

            for (int i = count; i > 0; i--)
            {
                var storageScheduleMessage = new StorageQueueMessage
                {
                    ClientRequestId = Guid.NewGuid().ToString(),
                    SchedulerJobId = i.ToString()
                };

                string message;
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, storageScheduleMessage);
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    message = sr.ReadToEnd();
                }

                var cloudMessage = new CloudQueueMessage(message);
                queue.Enqueue(cloudMessage);
            }

            queue.Enqueue(null);
            return queue;
        }
    }
}
