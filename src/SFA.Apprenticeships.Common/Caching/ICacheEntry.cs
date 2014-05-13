namespace SFA.Apprenticeships.Common.Caching
{
   internal interface ICacheEntry
    {
        CacheDuration Duration { get; }

        string Key<TFuncParam1>(TFuncParam1 funcParam1);
        string Key<TFuncParam1, TFuncParam2>(TFuncParam1 funcParam1, TFuncParam2 funcParam2);
        string Key<TFuncParam1, TFuncParam2, TFuncParam3>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3);
        string Key<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4);
        string Key<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5);
        string Key<TFuncParam1, TFuncParam2, TFuncParam3, TFuncParam4, TFuncParam5, TFuncParam6>(TFuncParam1 funcParam1, TFuncParam2 funcParam2, TFuncParam3 funcParam3, TFuncParam4 funcParam4, TFuncParam5 funcParam5, TFuncParam6 funcParam6);
    }
}
