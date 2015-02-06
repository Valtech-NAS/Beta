namespace SFA.Apprenticeships.Domain.Interfaces.Caching
{
    using System;

    public interface ICacheService
    {
        T Get<T>(string key) where T : class;

        TResult Get<TCacheEntry, TResult>(TCacheEntry cacheEntry, Func<TResult> dataFunc)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        void PutObject(string cacheKey, object cacheObject, CacheDuration cacheDuration = CacheDuration.CacheDefault);

        void Remove<TCacheEntry, TFuncParam1>(TCacheEntry cacheEntry, TFuncParam1 funcParam1)
            where TCacheEntry : BaseCacheKey;

        void FlushAll();
    }
}
