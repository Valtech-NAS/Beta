namespace SFA.Apprenticeships.Application.VacancyEtl.UnitTests.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Moq;
    using NUnit.Framework;
    using Domain.Interfaces.Mapping;
    using Interfaces.Messaging;
    using VacancyEtl;
    using Entities;
    using Domain.Entities.Vacancy;

    [TestFixture]
    public class VacancySummaryProcessorTests
    {
        private Mock<IMessageBus> _busMock;
        private Mock<IProcessControlQueue<StorageQueueMessage>> _messagingServiceMock;
        private Mock<IVacancyIndexDataProvider> _vacancyProviderMock;

        [SetUp]
        public void SetUp()
        {
            _busMock = new Mock<IMessageBus>();
            _messagingServiceMock = new Mock<IProcessControlQueue<StorageQueueMessage>>();
            _vacancyProviderMock = new Mock<IVacancyIndexDataProvider>();
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(4, 7)]
        [TestCase(10, 5)]
        public void ShouldQueueCorrectNumberOfVacancyPages(int nationalVacancyPages, int nonNationalVancanyPages)
        {
            _vacancyProviderMock.Setup(x => x.GetVacancyPageCount(VacancyLocationType.National)).Returns(nationalVacancyPages);
            _vacancyProviderMock.Setup(x => x.GetVacancyPageCount(VacancyLocationType.NonNational)).Returns(nonNationalVancanyPages);

            var vacancyConsumer = new VacancySummaryProcessor(_busMock.Object, _vacancyProviderMock.Object, _messagingServiceMock.Object);
            
            var scheduledMessage = new StorageQueueMessage()
            {
                ClientRequestId = Guid.NewGuid(),
                ExpectedExecutionTime = DateTime.Today,
                MessageId = "456",
                PopReceipt = "789"
            };
            vacancyConsumer.QueueVacancyPages(scheduledMessage);

            Thread.Sleep(100); //Slight delay to ensure parallel foreach has completed before assertions are made

            _messagingServiceMock.Verify(x => x.DeleteMessage(It.Is<string>(mid => mid == scheduledMessage.MessageId), It.Is<string>(pr => pr == scheduledMessage.PopReceipt)), Times.Once);
            _busMock.Verify(x => x.PublishMessage(It.IsAny<VacancySummaryPage>()), Times.Exactly(nationalVacancyPages + nonNationalVancanyPages));
        }

        [TestCase(VacancyLocationType.National, 0)]
        [TestCase(VacancyLocationType.NonNational, 5)]
        [TestCase(VacancyLocationType.Unknown, 10)]
        public void ShouldQueueCorrectNumberOfVacanySummaries(VacancyLocationType vacancyLocationType, int vacanciesReturned)
        {
            var summaryPage = new VacancySummaryPage { VacancyLocation = vacancyLocationType, PageNumber = vacanciesReturned, ScheduledRefreshDateTime = DateTime.Today };

            _vacancyProviderMock.Setup(x => x.GetVacancySummary(vacancyLocationType, vacanciesReturned))
                .Returns((VacancyLocationType vlt, int vr) =>
                {
                    var sumaries = new List<VacancySummary>(vr);
                    for (int i = 1; i <= vr; i++)
                    {
                        var summary = new VacancySummary { Id = i };
                        sumaries.Add(summary);
                    }
                    return sumaries;
                });

            var vacancyConsumer = new VacancySummaryProcessor(_busMock.Object, _vacancyProviderMock.Object, _messagingServiceMock.Object);
            
            vacancyConsumer.QueueVacancySummaries(summaryPage);

            _vacancyProviderMock.Verify(x => x.GetVacancySummary(
                                                It.Is<VacancyLocationType>(vlt => vlt == vacancyLocationType),
                                                It.Is<int>(pn => pn == vacanciesReturned)), 
                                                Times.Once);

            _busMock.Verify(x => x.PublishMessage(It.IsAny<VacancySummary>()), Times.Exactly(vacanciesReturned));
        }
    }
}
