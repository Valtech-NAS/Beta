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

    using VacancyErrorCodes = Application.Interfaces.Vacancies.ErrorCodes;
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
            var context = new { vacancyId };

            try
            {
                _logger.Debug("Calling Legacy.GetVacancyDetails for vacancy details with vacancy id='{0}'", vacancyId);

                var vacancyDetails = InternalGetVacancyDetails(vacancyId, errorIfNotFound);

                _logger.Debug(vacancyDetails == null ?
                    "Vacancy not found with vacancy id='{0}'. Returning null." :
                    "Legacy.GetVacancyDetails succeeded for vacancy details with vacancy id='{0}'", vacancyId);

                return vacancyDetails;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(VacancyErrorCodes.GetVacancyDetailsFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        #region Helpers

        private TVacancyDetail InternalGetVacancyDetails(int vacancyId, bool errorIfNotFound)
        {
            var request = new GetVacancyDetailsRequest
            {
                VacancyId = vacancyId
            };

            var response = default(GetVacancyDetailsResponse);

            _service.Use("SecureService", client => response = client.GetVacancyDetails(request).GetVacancyDetailsResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                string message;

                if (response == null)
                {
                    message = "No response";
                }
                else if (IsUnknownVacancy(response))
                {
                    if (errorIfNotFound)
                    {
                        throw new DomainException(VacancyErrorCodes.VacancyNotFoundError, new { vacancyId });
                    }

                    return null;                    
                }
                else
                {
                    message = string.Format("{0} validation error(s): {1}",
                        response.ValidationErrors.Count(), JsonConvert.SerializeObject(response, Formatting.None));
                }

                throw new DomainException(VacancyErrorCodes.GetVacancyDetailsFailed, new { message, vacancyId });
            }

            var vacancyDetail = _mapper.Map<Vacancy, TVacancyDetail>(response.Vacancy);

            // TODO: 1.6: consider relying entirely on VacancyStatus here and removing this check / update.
            if (HasClosingDatePassed(vacancyDetail) && vacancyDetail.VacancyStatus != VacancyStatuses.Expired)
            {
                _logger.Info("Vacancy ({0}) closing date {1} has passed. Setting status to {2} (was \"{3}\").",
                    vacancyId, vacancyDetail.ClosingDate, VacancyStatuses.Expired, vacancyDetail.VacancyStatus);

                vacancyDetail.VacancyStatus = VacancyStatuses.Expired;
            }

            return vacancyDetail;
        }

        private static bool IsUnknownVacancy(GetVacancyDetailsResponse response)
        {
            return response.ValidationErrors.Any(e => e.ErrorCode == UnknownVacancy);
        }

        private static bool HasClosingDatePassed(TVacancyDetail vacancyDetail)
        {
            return vacancyDetail.ClosingDate < DateTime.Today.ToUniversalTime();
        }

        #endregion
    }
}
