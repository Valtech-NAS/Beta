using System;

namespace SFA.Apprenticeships.Domain.Interfaces.Caching
{
    public enum CacheDuration
    {
        Midnight = 0,
        OneMinute = 1,
        FiveMinutes = 5,
        FifteenMinutes = 15,
        ThirtyMinutes = 30,
        OneHour = 60,
        OneDay = 1440,
    }
}
