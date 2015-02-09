namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.UnitTests
{
    using System;
    using Application.ApplicationUpdate;
    using Consumers;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;
    using Extensions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyStatusSummaryConsumerAsyncTests
    {
        private readonly Mock<ICacheService> _cacheServiceMock = new Mock<ICacheService>();
        private readonly Mock<IApplicationStatusProcessor> _applicationStatusProcessor = new Mock<IApplicationStatusProcessor>();
        private readonly VacancyStatusSummary _aVacancyStatusSummary = new VacancyStatusSummary { LegacyVacancyId = 123, ClosingDate = DateTime.Now.AddMonths(-3), VacancyStatus = VacancyStatuses.Expired };
        
        [SetUp]
        public void SetUp()
        {
            _cacheServiceMock.ResetCalls();
        }

        [Test]
        public void ShouldNotQueueWhenInCache()
        {
            _cacheServiceMock.Setup(x => x.Get<VacancyStatusSummary>(It.IsAny<string>())).Returns(new VacancyStatusSummary());
            var vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object, _applicationStatusProcessor.Object);

            var task = vacancyStatusSummaryConsumerAsync.Consume(new VacancyStatusSummary());
            task.Wait();

            _cacheServiceMock.Verify(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()), Times.Never);
            _applicationStatusProcessor.Verify(x => x.ProcessApplicationStatuses(It.IsAny<int>(), It.IsAny<VacancyStatuses>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public void ShouldPutInCacheWhenNotInCache()
        {
            var vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object, _applicationStatusProcessor.Object);
            var task = vacancyStatusSummaryConsumerAsync.Consume(_aVacancyStatusSummary);
            task.Wait();

            _cacheServiceMock.Verify(
                x =>
                    x.PutObject(
                        It.Is<string>(c => c == _aVacancyStatusSummary.CacheKey()),
                        It.Is<object>(vss => vss == _aVacancyStatusSummary),
                        It.Is<CacheDuration>(c => c == _aVacancyStatusSummary.CacheDuration())), Times.Once);

            _applicationStatusProcessor.Verify(
                x =>
                    x.ProcessApplicationStatuses(
                        It.Is<int>(i => i == _aVacancyStatusSummary.LegacyVacancyId),
                        It.Is<VacancyStatuses>(vs => vs == VacancyStatuses.Expired),
                        It.Is<DateTime>(cd => cd == _aVacancyStatusSummary.ClosingDate)), Times.Once);
        }
    }
}
