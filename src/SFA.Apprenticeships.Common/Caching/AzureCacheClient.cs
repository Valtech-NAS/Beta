namespace SFA.Apprenticeships.Common.Caching
{
    using System;
    using Microsoft.ApplicationServer.Caching;

    public class AzureCacheClient : ICacheClient
    {
        private DataCache cache;

        public AzureCacheClient()
        {
            var cacheFactory = new DataCacheFactory();
            cache = cacheFactory.GetDefaultCache();
        }

        private void Store(string key, object value, CacheDuration cacheDuration)
        {
            if (value == null)
            {
                return;
            }

            var timespan = TimeSpan.FromMinutes((int)cacheDuration);
            cache.Add(key, value, timespan);
        }

        public T Get<T>(string key) where T : class
        {
            var result = cache[key] as T;

            if (result != null)
            {
                return result;
            }

            return default(T);
        }

        private void Remove(string key)
        {
            cache.Remove(key);
        }

        public void FlushAll()
        {
            cache.Clear();
        }

        #region Get

        public TResult Get<TCacheEntry, TResult>(TCacheEntry cacheEntry, Func<TResult> dataFunc)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key();

            var result = cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc();
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheEntry, TFuncParam1, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1)
                        where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);

            var result = cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2);

            var result = cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3);

            var result = cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4);

            var result = cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);

            var result = cache[cacheKey] as TResult;
            if (result == null || result.Equals(default(TResult)))
            {
                result = dataFunc(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);
                Store(cacheKey, result, cacheEntry.Duration);
                return result;
            }

            return result;
        }

        public TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);

            var result = cache[cacheKey] as TResult;
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

        public void Put<TCacheEntry, TResult, TFuncParam1>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        public void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheEntry : BaseCacheEntry
            where TResult : class
        {
            var cacheKey = cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6);
            Store(cacheKey, result, cacheEntry.Duration);
        }

        #endregion

        #region Remove

        public void Remove<TCacheEntry, TFuncParam1>(TCacheEntry cacheEntry, TFuncParam1 funcParam1) where TCacheEntry : BaseCacheEntry
        {
            Remove(cacheEntry.Key(funcParam1));
        }

        public void Remove<TCacheEntry, TFuncParam1, TFuncParam2>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2) where TCacheEntry : BaseCacheEntry
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2));
        }

        public void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3) where TCacheEntry : BaseCacheEntry
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3));
        }

        public void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4) where TCacheEntry : BaseCacheEntry
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4));
        }

        public void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5) where TCacheEntry : BaseCacheEntry
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5));
        }

        public void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6) where TCacheEntry : BaseCacheEntry
        {
            Remove(cacheEntry.Key(funcParam1, funcParam2, funcParam3, funcParam4, funcParam5, funcParam6));
        }

        #endregion
    }
}
