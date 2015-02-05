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

        #region Get

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

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                _logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            _logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        #endregion

        #region Put

        public void PutObject(string cacheKey, object cacheObject)
        {
            Store(cacheKey, cacheObject, CacheDuration.CacheDefault);
        }

        public void PutObject(string cacheKey, object cacheObject, CacheDuration cacheDuration)
        {
            Store(cacheKey, cacheObject, cacheDuration);
        }

        public void Put<TCacheKey, TResult, TFuncParam1>(TCacheKey cacheEntry, TResult result, TFuncParam1 funcParam1)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheKey, TResult, TFuncParam1, TFuncParam2>(TCacheKey cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheKey, TResult, TFuncParam1, TFuncParam2, TFuncParam3>(TCacheKey cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheKey, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TCacheKey cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheKey, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TCacheKey cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheKey, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TCacheKey cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        #endregion

        #region Remove

        public void Remove<TCacheKey, TFuncParam1>(TCacheKey cacheEntry, TFuncParam1 funcParam1) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1));
        }

        public void Remove<TCacheKey, TFuncParam1, TFuncParam2>(TCacheKey cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2));
        }

        public void Remove<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3>(TCacheKey cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3));
        }

        public void Remove<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TCacheKey cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4));
        }

        public void Remove<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TCacheKey cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5));
        }

        public void Remove<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TCacheKey cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6) where TCacheKey : BaseCacheKey
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6));
        }

        #endregion
    }
}
