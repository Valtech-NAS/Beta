namespace SFA.Apprenticeships.Common.Tests.Caching
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class MemoryCachingTests
    {
        private MemoryCacheClient _memoryCacheClient;
        private TestCacheKeyEntry _cacheKeyEntry;
        private TestCachedObject _testCachedObject;
        private Func<int, string, TestCachedObject> _testFunc;

        [SetUp]
        public void Setup()
        {
            _memoryCacheClient = new MemoryCacheClient();
            _cacheKeyEntry = new TestCacheKeyEntry();
            _testCachedObject = new TestCachedObject() { DateTimeCached = DateTime.Now };
            _testFunc = ((i, s) => _testCachedObject);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryCacheClient.FlushAll();
        }

        [Test]
        public void AddsItemToCache()
        {
            var nullResult = _memoryCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();

            _memoryCacheClient.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _memoryCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);
        }

        [Test]
        public void RemovesItemFromCache()
        {
            _memoryCacheClient.Get(_cacheKeyEntry, _testFunc, 1, "2");
            var notNullResult = _memoryCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));

            notNullResult.Should().NotBe(null);
            notNullResult.DateTimeCached.Should().Be(_testCachedObject.DateTimeCached);

            _memoryCacheClient.Remove(_cacheKeyEntry, 1, "2");
            var nullResult = _memoryCacheClient.Get<TestCachedObject>(_cacheKeyEntry.Key(1, "2"));
            nullResult.Should().BeNull();
        }

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
