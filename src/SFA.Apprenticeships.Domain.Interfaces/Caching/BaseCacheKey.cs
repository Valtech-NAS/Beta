using System;

namespace SFA.Apprenticeships.Domain.Interfaces.Caching
{
    public abstract class BaseCacheKey
    {
        protected const string CacheKeySeparator = "_";

        protected abstract string KeyPrefix { get; }

        public abstract CacheDuration Duration { get; }

        public virtual string Key()
        {
            return MakeSafeKey(KeyPrefix);
        }

        public virtual string Key<TFuncParam1>(TFuncParam1 funcParam1)
        {
            return MakeSafeKey(KeyPrefix + funcParam1);
        }

        public virtual string Key<TFuncParam1, TFuncParam2>(TFuncParam1 funcParam1, TFuncParam2 funcParam2)
        {
            return MakeSafeKey(KeyPrefix + funcParam1 + CacheKeySeparator + funcParam2);
        }

        public virtual string Key<TFuncParam1, TFuncParam2, TFuncParam3>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3)
        {
            return MakeSafeKey(KeyPrefix + funcParam1 + CacheKeySeparator + funcParam2 + CacheKeySeparator + funcParam3);
        }

        public virtual string Key<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4)
        {
            return MakeSafeKey(KeyPrefix + funcParam1 + CacheKeySeparator + funcParam2 + CacheKeySeparator + funcParam3 + CacheKeySeparator + funcParam4);
        }

        public virtual string Key<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5)
        {
            return MakeSafeKey(KeyPrefix + funcParam1 + CacheKeySeparator + funcParam2 + CacheKeySeparator + funcParam3 + CacheKeySeparator + funcParam4 + CacheKeySeparator + funcParam5);
        }

        public virtual string Key<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6)
        {
            return MakeSafeKey(KeyPrefix + funcParam1 + CacheKeySeparator + funcParam2 + CacheKeySeparator + funcParam3 + CacheKeySeparator + funcParam4 + CacheKeySeparator + funcParam5 + CacheKeySeparator + funcParam6);
        }

        protected static string MakeSafeKey(string unsafeKey)
        {
            return unsafeKey.Replace(" ", CacheKeySeparator);
        }
    }
}
