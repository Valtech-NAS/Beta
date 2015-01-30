namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using Application.Interfaces.Logging;
    using Application.Vacancy;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;

    //TODO: replace once new NAS Gateway operation available
    public class CachedLegacyVacancyDataProvider<TVacancyDetail> : IVacancyDataProvider<TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        private readonly ILogService _logger;

        private static readonly BaseCacheKey VacancyDataCacheKey = new VacancyDataServiceCacheKeyEntry();
        private readonly ICacheService _cacheService;
        private readonly IVacancyDataProvider<TVacancyDetail> _vacancyDataProvider;

        public CachedLegacyVacancyDataProvider(ICacheService cacheService, IVacancyDataProvider<TVacancyDetail> vacancyDataProvider, ILogService logger)
        {
            _cacheService = cacheService;
            _vacancyDataProvider = vacancyDataProvider;
            _logger = logger;
        }

        public TVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound)
        {
            _logger.Debug("Calling GetVacancyDetails for VacancyId: {0}", vacancyId);
            return _cacheService.Get(VacancyDataCacheKey, vacancyId1 => _vacancyDataProvider.GetVacancyDetails(vacancyId1, errorIfNotFound), vacancyId);
        }
    }
}
