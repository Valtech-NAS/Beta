namespace SFA.Apprenticeships.Infrastructure.Caching.Azure
{
    using System;
    using Microsoft.ApplicationServer.Caching;
    using Domain.Interfaces.Caching;
    using NLog;

    public class AzureCacheService : ICacheService
    {
        private const string GettingItemFromCacheFormat = "Getting item with key: {0} from cache";
        private const string ItemReturnedFromCacheFormat = "Item with key: {0} returned from cache";
        private const string ItemNotInCacheFormat = "Item with key: {0} not in cache";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly DataCache _cache;

        public AzureCacheService()
        {
            var cacheFactory = new DataCacheFactory();
            _cache = cacheFactory.GetDefaultCache();
        }

        private void Store(string key, object value, CacheDuration cacheDuration)
        {
            Logger.Debug("Storing item with key: {0} in cache with duration: {1}", key, cacheDuration);

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
                    Logger.Warn("Attempt to store item in cache with key: " + key + " with expiry timespan: " + cacheTimeSpan, cacheException);
                }
                else
                {
                    Logger.Warn("Attempt to store item in cache with key: " + key + " using cache default eveiction policy", cacheException);
                }
                
                return;
            }

            Logger.Debug("Stored item with key: {0} in cache with timespan: {1}", key, cacheTimeSpan);
        }

        public T Get<T>(string key) where T : class
        {
            Logger.Debug(GettingItemFromCacheFormat, key);
            T result;
            
            try
            {
                result = _cache[key] as T;
            }
            catch (Exception cacheException)
            {
                var message = string.Format("Attempt to retreive item from cache with key {0} failed", key);
                Logger.Warn(message, cacheException);
                return null;
            }

            if (result == null)
            {
                Logger.Debug(ItemNotInCacheFormat, key);
            }

            return result;
        }

        private void Remove(string key)
        {
            Logger.Debug("Removing item with key: {0} from cache", key);

            try
            {
                _cache.Remove(key);
            }
            catch (Exception cacheException)
            {
                Logger.Warn("Attempt to remove item from cache with key: " + key, cacheException);
            }
        }

        public void FlushAll()
        {
            Logger.Debug("Flushing cache");

            try
            {
                _cache.Clear();
            }
            catch (Exception cacheException)
            {
                Logger.Warn("Attempt to clear cache failed", cacheException);
                return;
            }

            Logger.Debug("Flushed cache");
        }

        #region Get

        public TResult Get<TCacheKey, TResult>(TCacheKey cacheEntry, Func<TResult> dataFunc)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc();
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1)
                        where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2);

            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3);

            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4);
            
            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);

            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);

            Logger.Debug(GettingItemFromCacheFormat, cacheKey);

            var result = Get<TResult>(cacheKey);
            if (result == null || result.Equals(default(TResult)))
            {
                Logger.Debug(ItemNotInCacheFormat, cacheKey);

                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            Logger.Debug(ItemReturnedFromCacheFormat, cacheKey);

            return result;
        }

        #endregion

        #region Put

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
