namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using Application.Vacancy;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Caching;
    using NLog;

    //TODO: replace once new NAS Gateway operation available
    public class CachedLegacyVacancyDataProvider : IVacancyDataProvider
    {
        private static readonly BaseCacheKey VacancyDataCacheKey = new VacancyDataServiceCacheKeyEntry();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICacheService _cacheService;
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public CachedLegacyVacancyDataProvider(ICacheService cacheService, IVacancyDataProvider vacancyDataProvider)
        {
            _cacheService = cacheService;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public ApprenticeshipVacancyDetail GetVacancyDetails(int vacancyId)
        {
            Logger.Debug("Calling GetVacancyDetails for VacancyId: {0}", vacancyId);
            return _cacheService.Get(VacancyDataCacheKey, _vacancyDataProvider.GetVacancyDetails, vacancyId);
        }
    }
}