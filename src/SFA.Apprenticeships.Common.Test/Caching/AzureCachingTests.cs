namespace SFA.Apprenticeships.Common.Caching.Tests.Caching
{
    using System;
    using FluentAssertions;
    using Microsoft.ApplicationServer.Caching;
    using NUnit.Framework;
    using SFA.Apprenticeships.Common.Configuration;

    [TestFixture]
    public class AzureCachingTests
    {
        private AzureCacheClient _azureCacheClient;
        private TestCacheKeyEntry _cacheKeyEntry;
        private TestCachedObject _testCachedObject;
        private Func<int, string, TestCachedObject> _testFunc;

        [SetUp]
        public void Setup()
        {
            _azureCacheClient = new AzureCacheClient();
            _cacheKeyEntry = new TestCacheKeyEntry();
            _testCachedObject = new TestCachedObject() { DateTimeCached = DateTime.Now };
            _testFunc = ((i, s) => _testCachedObject);
        }

        [TearDown]
        public void TearDown()
        {
            _azureCacheClient.FlushAll();
        }

        [Test]
        public void AddsItemToCache()
        {
            var nullResult = _azureCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();

            _azureCacheClient.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _azureCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);
        }

        [Test]
        public void RemovesItemFromCache()
        {
            _azureCacheClient.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _azureCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);

            _azureCacheClient.Remove(_cacheKeyEntry, 1, "2");
            var nullResult = _azureCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();
        }

        [Serializable]
        private class TestCachedObject
        {
            public DateTime DateTimeCached { get; set; }
        }

        private class TestCacheKeyEntry : BaseCacheEntry
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
