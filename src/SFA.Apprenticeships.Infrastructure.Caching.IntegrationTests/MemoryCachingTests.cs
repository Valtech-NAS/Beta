namespace SFA.Apprenticeships.Infrastructure.Caching.IntegrationTests
{
    using System;
    using Application.Interfaces.Logging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Domain.Interfaces.Caching;
    using Memory;

    [TestFixture]
    public class MemoryCachingTests
    {
        private MemoryCacheService _memoryCacheService;
        private TestCacheKeyEntry _cacheKeyEntry;
        private TestCachedObject _testCachedObject;
        private Func<int, string, TestCachedObject> _testFunc;

        [SetUp]
        public void Setup()
        {
            _memoryCacheService = new MemoryCacheService(new Mock<ILogService>().Object);
            _cacheKeyEntry = new TestCacheKeyEntry();
            _testCachedObject = new TestCachedObject { DateTimeCached = DateTime.Now };
            _testFunc = ((i, s) => _testCachedObject);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCacheService.FlushAll();
        }

        [Test, Category("Integration")]
        public void AddsItemToCache()
        {
            var nullResult = _memoryCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();

            _memoryCacheService.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _memoryCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);
        }

        [Test, Category("Integration")]
        public void RemovesItemFromCache()
        {
            _memoryCacheService.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _memoryCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);

            _memoryCacheService.Remove(_cacheKeyEntry, 1, "2");
            var nullResult = _memoryCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();
        }

        private class TestCachedObject
        {
            public DateTime DateTimeCached { get; set; }
        }

        private class TestCacheKeyEntry : BaseCacheKey
        {
            protected override string KeyPrefix
            {
                get { return "TestKeyPrefix"; }
            }

            public override CacheDuration Duration
            {
                get { return CacheDuration.OneMinute; }
            }
        }
    }
}
