namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using Application.Vacancy;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;
    using NLog;

    //TODO: replace once new NAS Gateway operation available
    public class CachedLegacyVacancyDataProvider<TVacancyDetail> : IVacancyDataProvider<TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        private static readonly BaseCacheKey VacancyDataCacheKey = new VacancyDataServiceCacheKeyEntry();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICacheService _cacheService;
        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;

        public CachedLegacyVacancyDataProvider(ICacheService cacheService, IVacancyDataProvider<TVacancyDetail> vacancyDataProvider)
        {
            _cacheService = cacheService;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public TVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound)
        {
            Logger.Debug("Calling GetVacancyDetails for VacancyId: {0}", vacancyId);
            return _cacheService.Get(VacancyDataCacheKey, vacancyId1 => _vacancyDataProvider.GetVacancyDetails(vacancyId1, errorIfNotFound), vacancyId);
        }
    }
}