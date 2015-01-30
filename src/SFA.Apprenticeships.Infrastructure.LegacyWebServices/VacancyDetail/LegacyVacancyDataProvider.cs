namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using System;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Vacancy;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using Wcf;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;
    using MessagingErrorCodes = Application.Interfaces.Messaging.ErrorCodes;

    public class LegacyVacancyDataProvider<TVacancyDetail> : IVacancyDataProvider<TVacancyDetail> where TVacancyDetail : VacancyDetail
    {
        private const string UnknownVacancy = "UNKNOWN_VACANCY";
        private readonly ILogService _logger;
        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyVacancyDataProvider(
            IWcfService<GatewayServiceContract> service,
            IMapper mapper, ILogService logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        public TVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound)
        {
            var request = new GetVacancyDetailsRequest { VacancyId = vacancyId };

            var response = default(GetVacancyDetailsResponse);

            _logger.Debug("Calling Legacy.GetVacancyDetails webservice for vacancy details with ID {0}", vacancyId);

            _service.Use("SecureService", client => response = client.GetVacancyDetails(request).GetVacancyDetailsResponse);

            if (IsThereAnyErrorOn(response))
            {
                if (IsThereAnyValidationErrorOn(response))
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    _logger.Info("Legacy.GetVacancyDetails reported {0} validation error(s): {1}", response.ValidationErrors.Count(), responseAsJson);

                    if (VacancyDoesntExist(response))
                    {
                        if (errorIfNotFound)
                        {
                            throw new CustomException("Vacancy not found with ID {0}.", MessagingErrorCodes.VacancyNotFoundError, vacancyId);
                        }

                        _logger.Info("Vacancy not found with ID {0}. Returning null.", vacancyId);
                        return null;
                    }
                }
                else
                {
                    _logger.Info("Legacy.GetVacancyDetails did not respond");
                }

                var message = string.Format("Legacy.GetVacancyDetails failed to retrieve vacancy details from legacy system for vacancyId {0}", vacancyId);

                throw new CustomException(message, ErrorCodes.GatewayServiceFailed);
            }

            var vacancyDetail = GetVacancyDetailFrom(response);

            if (HasClosingDatePassed(vacancyDetail) && vacancyDetail.VacancyStatus != VacancyStatuses.Expired)
            {
                _logger.Info("Vacancy ({0}) closing date {1} has passed. Setting status to {2} (was \"{3}\").",
                    vacancyId, vacancyDetail.ClosingDate, VacancyStatuses.Expired, vacancyDetail.VacancyStatus);

                vacancyDetail.VacancyStatus = VacancyStatuses.Expired;
            }

            return vacancyDetail;
        }

        private static bool VacancyDoesntExist(GetVacancyDetailsResponse response)
        {
            return response.ValidationErrors.Any(e => e.ErrorCode == UnknownVacancy);
        }

        private static bool HasClosingDatePassed(TVacancyDetail vacancyDetail)
        {
            return vacancyDetail.ClosingDate < DateTime.Today.ToUniversalTime();
        }

        private TVacancyDetail GetVacancyDetailFrom(GetVacancyDetailsResponse response)
        {
            return _mapper.Map<Vacancy, TVacancyDetail>(response.Vacancy);
        }

        private static bool IsThereAnyValidationErrorOn(GetVacancyDetailsResponse response)
        {
            return response != null && response.ValidationErrors != null && response.ValidationErrors.Any();
        }

        private static bool IsThereAnyErrorOn(GetVacancyDetailsResponse response)
        {
            return response == null || response.Vacancy == null || (response.ValidationErrors != null && response.ValidationErrors.Any());
        }
    }
}
