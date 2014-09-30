namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using System;
    using System.Linq;
    using Application.Vacancy;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using NLog;
    using Wcf;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class LegacyVacancyDataProvider : IVacancyDataProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyVacancyDataProvider(
            IWcfService<GatewayServiceContract> service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            var request = new GetVacancyDetailsRequest {VacancyId = vacancyId};

            Logger.Debug("Calling Gateway GetVacancyDetails webservice for vacancy details with ID {0}", vacancyId);

            var response = default(GetVacancyDetailsResponse);

            //todo: remove endpoint config name once all new service operations integrated
            _service.Use("SecureService", client => response = client.GetVacancyDetails(request).GetVacancyDetailsResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    Logger.Info("Gateway GetVacancyDetails reported {0} validation error(s): {1}",
                        response.ValidationErrors.Count(), responseAsJson);
                }
                else
                {
                    Logger.Info("Gateway GetVacancyDetails did not respond");
                }

                var message =
                    string.Format(
                        "Gateway GetVacancyDetails failed to retrieve vacancy details from legacy system for vacancyId {0}",
                        vacancyId);
                throw new CustomException(
                    message,
                    ErrorCodes.GatewayServiceFailed);
            }

            var vacancyDetail = _mapper.Map<Vacancy, VacancyDetail>(response.Vacancy);

            if (vacancyDetail.ClosingDate < DateTime.Today.ToUniversalTime())
            {
                // Vacancy has expired.
                Logger.Debug("Vacancy has expired. Returning null.");
                return null;
            }

            return vacancyDetail;
        }
    }
}
