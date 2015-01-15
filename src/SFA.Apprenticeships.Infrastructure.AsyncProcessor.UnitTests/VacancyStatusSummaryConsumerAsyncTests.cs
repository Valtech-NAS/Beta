namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.UnitTests
{
    using Consumers;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
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
        private Mock<IMessageBus> _bus;

        [TestFixtureSetUp]
        private void SetUp()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _apprenticeshipApplicationReadMock = new Mock<IApprenticeshipApplicationReadRepository>();
            _bus = new Mock<IMessageBus>();
            _vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object, _apprenticeshipApplicationReadMock.Object, _bus.Object);
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
            var vacancyStatusSummary = new VacancyStatusSummary() { LegacyVacancyId = 123};
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
        public void ShouldQueueApplicationStatusSummaryForEachApprenticeshipApplication()
        {
            var applicationSummaries = Builder<ApplicationStatusSummary>.CreateListOfSize(5).Build();

            _bus.Setup(x => x.PublishMessage(It.IsAny<ApplicationStatusSummary>()));

            _apprenticeshipApplicationReadMock.Setup(
                x => x.GetApprenticeshipApplications(It.IsAny<int>(), It.IsAny<VacancyStatuses>()))
                .Returns(applicationSummaries);
            
            var vacancyStatusSummary = new VacancyStatusSummary() { LegacyVacancyId = 123, VacancyStatus = VacancyStatuses.Unavailable};

            var task = _vacancyStatusSummaryConsumerAsync.Consume(vacancyStatusSummary);
            task.Wait();

            _apprenticeshipApplicationReadMock.Verify(
                x =>
                    x.GetApprenticeshipApplications(
                        It.Is<int>(id => id == vacancyStatusSummary.LegacyVacancyId),
                        It.Is<VacancyStatuses>(vs => vs == VacancyStatuses.Unavailable)), Times.Once);

            _bus.Verify(x => x.PublishMessage(It.IsAny<ApplicationStatusSummary>()), Times.Exactly(5));
        }
    }
}
