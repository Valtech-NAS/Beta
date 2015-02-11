namespace SFA.Apprenticeships.Infrastructure.Caching.Azure
{
    using System;
    using Application.Interfaces.Logging;
    using Microsoft.ApplicationServer.Caching;
    using Domain.Interfaces.Caching;

    public class AzureCacheService : ICacheService
    {
        private readonly ILogService _logger;

        private const string GettingItemFromCacheFormat = "Getting item with key: {0} from cache";
        private const string ItemReturnedFromCacheFormat = "Item with key: {0} returned from cache";
        private const string ItemNotInCacheFormat = "Item with key: {0} not in cache";

        private readonly DataCache _cache;

        public AzureCacheService(ILogService logger)
        {
            _logger = logger;
            var cacheFactory = new DataCacheFactory();
            _cache = cacheFactory.GetDefaultCache();
        }

        private void Store(string key, object value, CacheDuration cacheDuration)
        {
            _logger.Debug("Storing item with key: {0} in cache with duration: {1}", key, cacheDuration);

            if (value == null)
            {
                return;
            }

            TimeSpan cacheTimeSpan = TimeSpan.Zero;

            try
            {
                if (cacheDuration != CacheDuration.CacheDefault)
                {
                    cacheTimeSpan = TimeSpan.FromMinutes((int)cacheDuration);
                    _cache.Put(key, value, cacheTimeSpan);
                }
                else
                {
                    _cache.Put(key, value);
                }
                
            }
            catch (Exception cacheException)
            {
                if (cacheTimeSpan != TimeSpan.Zero) 
                {
                    _logger.Warn("Attempt to store item in cache with key: " + key + " with expiry timespan: " + cacheTimeSpan, cacheException);
                }
                else
                {
                    _logger.Warn("Attempt to store item in cache with key: " + key + " using cache default eveiction policy", cacheException);
                }
                
                return;
            }

            _logger.Debug("Stored item with key: {0} in cache with timespan: {1}", key, cacheTimeSpan);
        }

        public T Get<T>(string key) where T : class
        {
            _logger.Debug(GettingItemFromCacheFormat, key);
            T result;
            
            try
            {
                result = _cache[key] as T;
            }
            catch (Exception cacheException)
            {
                var message = string.Format("Attempt to retreive item from cache with key {0} failed", key);
                _logger.Warn(message, cacheException);
                return null;
            }

            if (result == null)
            {
                _logger.Debug(ItemNotInCacheFormat, key);
            }

            _logger.Debug(ItemReturnedFromCacheFormat, key);

            return result;
        }

        private void Remove(string key)
        {
            _logger.Debug("Removing item with key: {0} from cache", key);

            try
            {
                _cache.Remove(key);
            }
            catch (Exception cacheException)
            {
                _logger.Warn("Attempt to remove item from cache with key: " + key, cacheException);
            }
        }

        public void FlushAll()
        {
            _logger.Debug("Flushing cache");

            try
            {
                _cache.Clear();
            }
            catch (Exception cacheException)
            {
                _logger.Warn("Attempt to clear cache failed", cacheException);
                return;
            }

            _logger.Debug("Flushed cache");
        }

        public TResult Get<TCacheKey, TResult>(TCacheKey cacheEntry, Func<TResult> dataFunc)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            _logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
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

            var result = Get<TResult>(cacheKey);
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
