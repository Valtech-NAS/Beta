namespace SFA.Apprenticeships.Infrastructure.Caching.Memory
{
    using System;
    using System.Linq;
    using System.Runtime.Caching;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Caching;

    public class MemoryCacheService : ICacheService
    {
        private readonly ILogService _logger;
        private const string GettingItemFromCacheFormat = "Getting item with key: {0} from cache";
        private const string ItemReturnedFromCacheFormat = "Item with key: {0} returned from cache";
        private const string ItemNotInCacheFormat = "Item with key: {0} not in cache";

        private readonly ObjectCache _cache;
        public MemoryCacheService(ILogService logger)
        {
            _logger = logger;
            _cache = MemoryCache.Default;
        }

        private void Store(string key, object value, CacheDuration cacheDuration)
        {
            _logger.Debug("Storing item with key: {0} in cache with duration: {1}", key, cacheDuration);

            if (value == null)
            {
                return;
            }

            TimeSpan cacheTimeSpan = TimeSpan.FromMinutes((int)cacheDuration);

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(cacheTimeSpan) };
            _cache.Set(key, value, policy);

            _logger.Debug("Stored item with key: {0} in cache with timespan: {1}", key, cacheTimeSpan);
        }

        public T Get<T>(string key) where T : class
        {
            _logger.Debug(GettingItemFromCacheFormat, key);

            var result = _cache[key] as T;

            if (result == null)
            {
                _logger.Debug(ItemNotInCacheFormat, key);
            }

            return result;
        }

        private void Remove(string key)
        {
            _logger.Debug("Removing item with key: {0} from cache", key);

            _cache.Remove(key);

            _logger.Debug("Removed item with key: {0} from cache", key);
        }

        public void FlushAll()
        {
            _logger.Debug("Flushing cache");
            
            var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (var cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }

            _logger.Debug("Flushed cache");
        }

        public TResult Get<TCacheKey, TResult>(TCacheKey cacheEntry, Func<TResult> dataFunc)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc();
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public void PutObject(string cacheKey, object cacheObject, CacheDuration cacheDuration = CacheDuration.CacheDefault)
        {
            Store(cacheKey, cacheObject, cacheDuration);
        }

        public void Remove<TCacheKey, TFuncParam1>(TCacheKey cacheEntry, TFuncParam1 funcParam1) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1));
        }
    }
}
