namespace SFA.Apprenticeships.Domain.Interfaces.Caching
{
    using System;

    public interface ICacheService
    {
        #region Get

        T Get<T>(string key) where T : class;

        TResult Get<TCacheEntry, TResult>(TCacheEntry cacheEntry, Func<TResult> dataFunc)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TResult> dataFunc, TFuncParam1 funcParam1)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        TResult Get<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult>(TCacheEntry cacheEntry, Func<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6, TResult> dataFunc, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        #endregion

        #region Put

        void Put<TCacheEntry, TResult, TFuncParam1>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        void Put<TCacheEntry, TResult, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TCacheEntry cacheEntry, TResult result, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheEntry : BaseCacheKey
            where TResult : class;

        #endregion

        #region Remove

        void Remove<TCacheEntry, TFuncParam1>(TCacheEntry cacheEntry, TFuncParam1 funcParam1)
            where TCacheEntry : BaseCacheKey;

        void Remove<TCacheEntry, TFuncParam1, TFuncParam2>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2)
            where TCacheEntry : BaseCacheKey;

        void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
            where TCacheEntry : BaseCacheKey;

        void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
            where TCacheEntry : BaseCacheKey;

        void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
            where TCacheEntry : BaseCacheKey;

        void Remove<TCacheEntry, TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TCacheEntry cacheEntry, TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
            where TCacheEntry : BaseCacheKey;

        #endregion

        void FlushAll();
    }
}
