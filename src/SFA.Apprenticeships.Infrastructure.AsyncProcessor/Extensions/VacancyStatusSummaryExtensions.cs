namespace SFA.Apprenticeships.Infrastructure.AsyncProcessor.Extensions
{
    using Application.VacancyEtl.Entities;
    using Domain.Interfaces.Caching;

    public static class VacancyStatusSummaryExtensions
    {
        public static string CacheKey(this VacancyStatusSummary vacancyStatusSummary)
        {
            return vacancyStatusSummary != null
                ? string.Format("VacancyStatusSummary_CacheKey_{0}", vacancyStatusSummary.LegacyVacancyId)
                : null;
        }

        public static CacheDuration CacheDuration(this VacancyStatusSummary vacancyStatusSummary)
        {
            return Domain.Interfaces.Caching.CacheDuration.ThirtyMinutes;
        }
    }
}
