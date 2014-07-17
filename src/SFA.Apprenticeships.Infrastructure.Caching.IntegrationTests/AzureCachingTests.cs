namespace SFA.Apprenticeships.Infrastructure.Caching.IntegrationTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Domain.Interfaces.Caching;
    using Azure;

    [TestFixture]
    public class AzureCachingTests
    {
        private AzureCacheService _azureCacheService;
        private TestCacheKeyEntry _cacheKeyEntry;
        private TestCachedObject _testCachedObject;
        private Func<int, string, TestCachedObject> _testFunc;

        [SetUp]
        public void Setup()
        {
            _azureCacheService = new AzureCacheService();
            _cacheKeyEntry = new TestCacheKeyEntry();
            _testCachedObject = new TestCachedObject { DateTimeCached = DateTime.Now };
            _testFunc = ((i, s) => _testCachedObject);
        }

        [TearDown]
        public void TearDown()
        {
            _azureCacheService.FlushAll();
        }

        [Test]
        public void AddsItemToCache()
        {
            var nullResult = _azureCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();

            _azureCacheService.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _azureCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);
        }

        [Test]
        public void RemovesItemFromCache()
        {
            _azureCacheService.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _azureCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);

            _azureCacheService.Remove(_cacheKeyEntry, 1, "2");
            var nullResult = _azureCacheService.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();
        }

        [Serializable]
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
