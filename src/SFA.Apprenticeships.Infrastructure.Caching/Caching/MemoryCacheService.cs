namespace SFA.Apprenticeships.Infrastructure.Caching.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using SFA.Apprenticeships.Domain.Interfaces.Services.Caching;

    public class MemoryCacheService : ICacheService
    {
        private readonly ObjectCache _cache;

        public MemoryCacheService()
        {
            _cache = MemoryCache.Default;
        }

        private void Store(string key, object value, CacheDuration cacheDuration)
        {
            if (value == null)
            {
                return;
            }

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes((int)cacheDuration) };
            _cache.Set(key, value, policy);
        }

        public T Get<T>(string key) where T : class
        {
            var result = _cache[key] as T;

            if (result != null)
            {
                return result;
            }

            return default(T);
        }

        private void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void FlushAll()
        {
            List<string> cacheKeys = Enumerable.Select<KeyValuePair<string, object>, string>(MemoryCache.Default, kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
        }

        #region Get

        public TResult Get<TCacheKey, TResult>(TCacheKey cacheEntry, Func<TResult> dataFunc)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc();
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheKey, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult>(TCacheKey cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheKey : BaseCacheKey
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);

            var result = _cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

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
