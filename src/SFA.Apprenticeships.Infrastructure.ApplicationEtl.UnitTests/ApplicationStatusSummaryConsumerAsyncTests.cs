namespace SFA.Apprenticeships.Infrastructure.ApplicationEtl.UnitTests
{
    using System;
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Entities;
    using Application.VacancyEtl.Entities;
    using Consumers;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationStatusSummaryConsumerAsyncTests
    {
        private ApplicationStatusSummaryConsumerAsync _applicationStatusSummaryConsumerAsync;
        private Mock<IApplicationStatusProcessor> _applicationStatusProcessorMock;
        private Mock<IMessageBus> _bus;

        [SetUp]
        public void SetUp()
        {
            _applicationStatusProcessorMock = new Mock<IApplicationStatusProcessor>();
            _bus = new Mock<IMessageBus>();
            _applicationStatusSummaryConsumerAsync = new ApplicationStatusSummaryConsumerAsync(_applicationStatusProcessorMock.Object, _bus.Object);
        }

        [Test]
        public void ConsumeShouldProcessMessageCorrectly()
        {
            _applicationStatusProcessorMock.Setup(x => x.ProcessApplicationStatuses(It.IsAny<ApplicationStatusSummary>()));
            _bus.Setup(x => x.PublishMessage(It.IsAny<VacancyStatusSummary>()));

            var appStatusSummary = new ApplicationStatusSummary
            {
                ApplicationId = Guid.NewGuid(),
                LegacyVacancyId = 101,
                ApplicationStatus = ApplicationStatuses.Submitted,
                ApplicationType = ApplicationType.TraineeshipApplication,
                VacancyStatus = VacancyStatuses.Live,
                ClosingDate = DateTime.Now,
                LegacyApplicationId = 1001,
                UnsuccessfulReason = "Because"
            };

            var task = _applicationStatusSummaryConsumerAsync.Consume(appStatusSummary);
            task.Wait();

            _applicationStatusProcessorMock.Verify(x => x.ProcessApplicationStatuses(It.Is<ApplicationStatusSummary>(y => y == appStatusSummary)));
            _bus.Verify(
                x =>
                    x.PublishMessage(
                        It.Is<VacancyStatusSummary>(
                            v =>
                                v.LegacyVacancyId == appStatusSummary.LegacyVacancyId &&
                                v.ClosingDate == appStatusSummary.ClosingDate &&
                                v.DateTime < DateTime.Now &&
                                v.VacancyStatus == appStatusSummary.VacancyStatus)));

        }
    }
}
