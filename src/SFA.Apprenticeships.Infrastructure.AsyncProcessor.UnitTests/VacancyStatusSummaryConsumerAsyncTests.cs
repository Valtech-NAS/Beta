namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.UnitTests
{
    using Application.ApplicationUpdate.Entities;
    using Application.VacancyEtl.Entities;
    using Consumers;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using Extensions;
    using FizzWare.NBuilder;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyStatusSummaryConsumerAsyncTests
    {
        private VacancyStatusSummaryConsumerAsync _vacancyStatusSummaryConsumerAsync;
        private Mock<ICacheService> _cacheServiceMock;
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadMock;
        private Mock<ITraineeshipApplicationReadRepository> _traineeshipApplicationReadMock;
        private Mock<IMessageBus> _bus;

        [SetUp]
        public void SetUp()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _apprenticeshipApplicationReadMock = new Mock<IApprenticeshipApplicationReadRepository>();
            _traineeshipApplicationReadMock = new Mock<ITraineeshipApplicationReadRepository>();
            _bus = new Mock<IMessageBus>();
            _vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object,
                _apprenticeshipApplicationReadMock.Object, _traineeshipApplicationReadMock.Object, _bus.Object);
        }

        [Test]
        public void ShouldNotQueueWhenInCache()
        {
            _cacheServiceMock.Setup(x => x.Get<VacancyStatusSummary>(It.IsAny<string>())).Returns(new VacancyStatusSummary());

            var task = _vacancyStatusSummaryConsumerAsync.Consume(new VacancyStatusSummary());
            task.Wait();

            _cacheServiceMock.Verify(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()), Times.Never);
        }

        [Test]
        public void ShouldPutInCacheWhenNotInCache()
        {
            var vacancyStatusSummary = new VacancyStatusSummary { LegacyVacancyId = 123};
            _cacheServiceMock.Setup(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()));

            var task = _vacancyStatusSummaryConsumerAsync.Consume(vacancyStatusSummary);
            task.Wait();

            _cacheServiceMock.Verify(
                x =>
                    x.PutObject(
                        It.Is<string>(c => c == vacancyStatusSummary.CacheKey()),
                        It.Is<object>(vss => vss == vacancyStatusSummary),
                        It.Is<CacheDuration>(c => c == vacancyStatusSummary.CacheDuration())), Times.Once);
        }

        [Test]
        public void ShouldQueueApplicationStatusSummaryForEachApplication()
        {
            //Would never get back from both app and trn but just checking right things are called
            var apprenticeshipApplicationSummaries = Builder<ApprenticeshipApplicationSummary>.CreateListOfSize(4).Build();
            var traineeshipApplicationSummaries = Builder<TraineeshipApplicationSummary>.CreateListOfSize(3).Build();

            _bus.Setup(x => x.PublishMessage(It.IsAny<ApplicationStatusSummary>()));

            _apprenticeshipApplicationReadMock.Setup(
                x => x.GetApplicationSummaries(It.IsAny<int>()))
                .Returns(apprenticeshipApplicationSummaries);

            _traineeshipApplicationReadMock.Setup(
                x => x.GetApplicationSummaries(It.IsAny<int>()))
                .Returns(traineeshipApplicationSummaries);
            
            var vacancyStatusSummary = new VacancyStatusSummary() { LegacyVacancyId = 123 };

            var task = _vacancyStatusSummaryConsumerAsync.Consume(vacancyStatusSummary);
            task.Wait();

            _apprenticeshipApplicationReadMock.Verify(
                x =>
                    x.GetApplicationSummaries(
                        It.Is<int>(id => id == vacancyStatusSummary.LegacyVacancyId)), Times.Once);

            _traineeshipApplicationReadMock.Verify(
                x =>
                    x.GetApplicationSummaries(
                        It.Is<int>(id => id == vacancyStatusSummary.LegacyVacancyId)), Times.Once);

            _bus.Verify(x => x.PublishMessage(It.IsAny<ApplicationStatusSummary>()), Times.Exactly(7));
        }
    }
}
