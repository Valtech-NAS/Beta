namespace SFA.Apprenticeships.Application.UnitTests.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Messaging;
    using Moq;
    using NUnit.Framework;
    using VacancyEtl;
    using VacancyEtl.Entities;

    [TestFixture]
    public class VacancySummaryProcessorTests
    {
        [SetUp]
        public void SetUp()
        {
            _busMock = new Mock<IMessageBus>();
            _messagingServiceMock = new Mock<IProcessControlQueue<StorageQueueMessage>>();
            _mapperMock = new Mock<IMapper>();
            _vacancyProviderMock = new Mock<IVacancyIndexDataProvider>();
            _configurationManagerMock = new Mock<IConfigurationManager>();
        }

        private Mock<IMessageBus> _busMock;
        private Mock<IProcessControlQueue<StorageQueueMessage>> _messagingServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IVacancyIndexDataProvider> _vacancyProviderMock;
        private Mock<IConfigurationManager> _configurationManagerMock;

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(10)]
        public void ShouldQueueCorrectNumberOfVacancyPages(int vancanyPages)
        {
            _vacancyProviderMock.Setup(x => x.GetVacancyPageCount()).Returns(vancanyPages);

            var vacancyConsumer = GetGatewayVacancySummaryProcessor();

            var scheduledMessage = new StorageQueueMessage
            {
                ClientRequestId = Guid.NewGuid(),
                ExpectedExecutionTime = DateTime.Today,
                MessageId = "456",
                PopReceipt = "789"
            };

            vacancyConsumer.QueueVacancyPages(scheduledMessage);

            Thread.Sleep(100); //Slight delay to ensure parallel foreach has completed before assertions are made

            _messagingServiceMock.Verify(
                x =>
                    x.DeleteMessage(It.Is<string>(mid => mid == scheduledMessage.MessageId),
                        It.Is<string>(pr => pr == scheduledMessage.PopReceipt)), Times.Once);
            _busMock.Verify(x => x.PublishMessage(It.IsAny<VacancySummaryPage>()), Times.Exactly(vancanyPages));
        }

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldQueueCorrectNumberOfVacanySummaries(int vacanciesReturned)
        {
            var summaryPage = new VacancySummaryPage
            {
                PageNumber = vacanciesReturned,
                ScheduledRefreshDateTime = DateTime.Today
            };

            _vacancyProviderMock.Setup(x => x.GetVacancySummaries(vacanciesReturned))
                .Returns((int vr) =>
                {
                    var traineeshipSummaries = new List<TraineeshipSummary>(vr);
                    var apprenticeshipSummaries = new List<ApprenticeshipSummary>(vr);
                    for (int i = 1; i <= vr; i++)
                    {
                        var apprenticeshipSummary = new ApprenticeshipSummary {Id = i};
                        apprenticeshipSummaries.Add(apprenticeshipSummary);
                    }
                    return new VacancySummaries(apprenticeshipSummaries, traineeshipSummaries);
                });

            _mapperMock.Setup(
                x =>
                    x.Map<IEnumerable<ApprenticeshipSummary>, IEnumerable<ApprenticeshipSummaryUpdate>>(
                        It.IsAny<IEnumerable<ApprenticeshipSummary>>()))
                .Returns((IEnumerable<ApprenticeshipSummary> vacancies) =>
                {
                    var vacUpdates = new List<ApprenticeshipSummaryUpdate>(vacancies.Count());
                    vacancies.ToList()
                        .ForEach(
                            v =>
                                vacUpdates.Add(new ApprenticeshipSummaryUpdate
                                {
                                    Id = v.Id,
                                    ScheduledRefreshDateTime = summaryPage.ScheduledRefreshDateTime
                                }));
                    return vacUpdates;
                });

            var vacancyConsumer = GetGatewayVacancySummaryProcessor();

            vacancyConsumer.QueueVacancySummaries(summaryPage);

            _vacancyProviderMock.Verify(x => x.GetVacancySummaries(
                It.Is<int>(pn => pn == vacanciesReturned)),
                Times.Once);
            _mapperMock.Verify(
                x =>
                    x.Map<IEnumerable<ApprenticeshipSummary>, IEnumerable<ApprenticeshipSummaryUpdate>>(
                        It.Is<IEnumerable<ApprenticeshipSummary>>(vc => vc.Count() == vacanciesReturned)), Times.Once);
            _busMock.Verify(x => x.PublishMessage(It.IsAny<ApprenticeshipSummary>()), Times.Exactly(vacanciesReturned));
        }

        private VacancySummaryProcessor GetGatewayVacancySummaryProcessor()
        {
            var vacancyConsumer = new VacancySummaryProcessor(_busMock.Object, _vacancyProviderMock.Object,
                _mapperMock.Object, _messagingServiceMock.Object, _configurationManagerMock.Object);
            return vacancyConsumer;
        }

        [Test]
        public void ShouldNotQueueTheVacancyIfTheVacancyIsNotAboutToExpire()
        {
            const int aVacancyId = 5;
            const int vacancyAboutToExpireNotificationHours = 96;
            _configurationManagerMock.Setup(cmm => cmm.GetAppSetting<int>("VacancyAboutToExpireNotificationHours"))
                .Returns(vacancyAboutToExpireNotificationHours);
            var vacancyConsumer = GetGatewayVacancySummaryProcessor();

            var vacancySummary = new ApprenticeshipSummary
            {
                Id = aVacancyId,
                ClosingDate = DateTime.Now.AddHours(vacancyAboutToExpireNotificationHours + 1)
            };

            vacancyConsumer.QueueVacancyIfExpiring(vacancySummary);

            _busMock.Verify(x => x.PublishMessage(It.Is<VacancyAboutToExpire>(m => m.Id == aVacancyId)), Times.Never());
            _configurationManagerMock.VerifyAll();
        }

        [Test]
        public void ShouldQueueTheVacancyIfTheVacancyIsAboutToExpire()
        {
            const int aVacancyId = 5;
            const int vacancyAboutToExpireNotificationHours = 96;
            _configurationManagerMock.Setup(cmm => cmm.GetAppSetting<int>("VacancyAboutToExpireNotificationHours"))
                .Returns(vacancyAboutToExpireNotificationHours);
            var vacancyConsumer = GetGatewayVacancySummaryProcessor();

            var vacancySummary = new ApprenticeshipSummary
            {
                Id = aVacancyId,
                ClosingDate = DateTime.Now.AddHours(vacancyAboutToExpireNotificationHours - 1)
            };

            vacancyConsumer.QueueVacancyIfExpiring(vacancySummary);

            _busMock.Verify(x => x.PublishMessage(It.Is<VacancyAboutToExpire>(m => m.Id == aVacancyId)));
            _configurationManagerMock.VerifyAll();
        }
    }
}