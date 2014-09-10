namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using System;
    using System.Linq;
    using Application.Vacancy;
    using Configuration;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using NLog;
    using VacancyDetailProxy;
    using Wcf;

    public class LegacyVacancyDataProvider : IVacancyDataProvider //todo: replace once new NAS Gateway operation available
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<IVacancyDetails> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly IMapper _mapper;

        public LegacyVacancyDataProvider(ILegacyServicesConfiguration legacyServicesConfiguration,
                                         IWcfService<IVacancyDetails> service,
                                         IMapper mapper)
        {
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _service = service;
            _mapper = mapper;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            var vacancyDetailRequest = new VacancyDetailsRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    VacancyReferenceId = vacancyId,
                    PageIndex = 1
                }
            };

            var rs = default(VacancyDetailsResponse);
            _service.Use(client => rs = client.Get(vacancyDetailRequest));

            if (rs == null ||
                rs.SearchResults == null ||
                rs.SearchResults.SearchResults == null ||
                rs.SearchResults.SearchResults.Length == 0)
            {
                Logger.Debug("No results returned from Legacy Vacancy Data Provider for VacancyId={0}", vacancyId);
                return default(VacancyDetail);
            }

            Logger.Debug("{0} results returned from Legacy Vacancy Data Provider for VacancyId={1}", rs.SearchResults.SearchResults.Count(), vacancyId);

            var vacancyDetail = _mapper.Map<VacancyFullData, VacancyDetail>(rs.SearchResults.SearchResults.First());

            if (vacancyDetail.ClosingDate < DateTime.Today.ToUniversalTime())
            {
                // Vacancy has expired.
                return null;
            }

            return vacancyDetail;
        }
    }
}
