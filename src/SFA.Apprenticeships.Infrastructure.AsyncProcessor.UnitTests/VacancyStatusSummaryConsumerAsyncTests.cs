namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.UnitTests
{
    using System;
    using Application.ApplicationUpdate;
    using Application.VacancyEtl.Entities;
    using Consumers;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;
    using Extensions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyStatusSummaryConsumerAsyncTests
    {
        private VacancyStatusSummaryConsumerAsync _vacancyStatusSummaryConsumerAsync;
        private Mock<ICacheService> _cacheServiceMock;
        private Mock<IApplicationStatusProcessor> _applicationStatusProcessor;

        [SetUp]
        public void SetUp()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _applicationStatusProcessor = new Mock<IApplicationStatusProcessor>();
            _vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object, _applicationStatusProcessor.Object);
        }

        [Test]
        public void ShouldNotQueueWhenInCache()
        {
            _applicationStatusProcessor.Setup(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()));
            _cacheServiceMock.Setup(x => x.Get<VacancyStatusSummary>(It.IsAny<string>())).Returns(new VacancyStatusSummary());

            var task = _vacancyStatusSummaryConsumerAsync.Consume(new VacancyStatusSummary());
            task.Wait();

            _cacheServiceMock.Verify(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()), Times.Never);
            _applicationStatusProcessor.Verify(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()), Times.Never);
        }

        [Test]
        public void ShouldPutInCacheWhenNotInCache()
        {
            var vacancyStatusSummary = new VacancyStatusSummary { LegacyVacancyId = 123, ClosingDate = DateTime.Now.AddMonths(-3), VacancyStatus = VacancyStatuses.Expired };
            _applicationStatusProcessor.Setup(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()));
            _cacheServiceMock.Setup(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()));

            var task = _vacancyStatusSummaryConsumerAsync.Consume(vacancyStatusSummary);
            task.Wait();

            _cacheServiceMock.Verify(
                x =>
                    x.PutObject(
                        It.Is<string>(c => c == vacancyStatusSummary.CacheKey()),
                        It.Is<object>(vss => vss == vacancyStatusSummary),
                        It.Is<CacheDuration>(c => c == vacancyStatusSummary.CacheDuration())), Times.Once);

            _applicationStatusProcessor.Verify(
                x =>
                    x.ProcessApplicationStatuses(
                        It.Is<VacancyStatusSummary>(i => i == vacancyStatusSummary)), Times.Once);
        }
    }
}
