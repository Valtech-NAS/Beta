namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.GetCandidateApplicationStatuses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.ApplicationUpdate.Entities;
    using Application.Interfaces.Logging;
    using Newtonsoft.Json;
    using Application.ApplicationUpdate;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class LegacyCandidateApplicationStatusesProvider : ILegacyApplicationStatusesProvider
    {
        private readonly ILogService _logger;

        private readonly IMapper _mapper;
        private readonly IWcfService<GatewayServiceContract> _service;
        private const int ApplicationStatusExtractWindow = 4*60; // todo: temp code to define 4 hour window for application ETL. should be from config

        public LegacyCandidateApplicationStatusesProvider(IWcfService<GatewayServiceContract> service, IMapper mapper, ILogService logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate)
        {
            var request = new GetCandidateInfoRequest
            {
                CandidateId = candidate.LegacyCandidateId
            };

            var response = default(GetCandidateInfoResponse);

            _logger.Debug("Calling Legacy.GetCandidateInfo for candidate '{0}'", candidate.EntityId);

            _service.Use("SecureService", client => response = client.GetCandidateInfo(request).GetCandidateInfoResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    _logger.Error("Legacy.GetCandidateInfo reported {0} validation error(s): {1}",
                        response.ValidationErrors.Count(), 
                        responseAsJson);
                }
                else
                {
                    _logger.Error("Legacy.GetCandidateInfo did not respond");
                }

                var message =
                    string.Format("Failed to retrieve applications from Legacy.GetCandidateInfo for candidate '{0}'/'{1}'",
                        candidate.EntityId,
                        candidate.LegacyCandidateId);
                throw new CustomException(message, ErrorCodes.GatewayServiceFailed);
            }

            _logger.Debug("Candidate applications were successfully retrieved from Legacy.GetCandidateInfo ({0})",
                response.CandidateApplications.Count());

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications);
        }

        public int GetApplicationStatusesPageCount()
        {
            // retrieve application statuses page count so can queue subsequent paged requests

            var request = new GetApplicationsStatusRequest
            {
                PageNumber = 1,
                RangeTo = DateTime.UtcNow,
                RangeFrom = DateTime.UtcNow.AddMinutes(ApplicationStatusExtractWindow * -1)
            };

            var response = default(GetApplicationsStatusResponse);

            _logger.Debug("Calling Legacy.GetApplicationsStatus for page count");

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                _logger.Error("Legacy.GetApplicationsStatus for page count did not respond");

                throw new CustomException("Failed to retrieve application status pages from Legacy.GetApplicationsStatus",
                    ErrorCodes.GatewayServiceFailed);
            }

            _logger.Debug("Application statuses page count retrieved from Legacy.GetApplicationsStatus ({0})", response.TotalPages);

            return response.TotalPages;
        }

        public IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int page)
        {
            // retrieve application statuses for ALL candidates (used in application ETL process)

            var request = new GetApplicationsStatusRequest
            {
                PageNumber = page,
                RangeTo = DateTime.UtcNow,
                RangeFrom = DateTime.UtcNow.AddMinutes(ApplicationStatusExtractWindow * -1)
            };

            var response = default(GetApplicationsStatusResponse);

            _logger.Debug("Calling Legacy.GetApplicationsStatus for page {0}", page);

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                _logger.Error("Legacy.GetApplicationsStatus did not respond");

                throw new CustomException("Failed to retrieve page '" + page + "' from Legacy.GetApplicationsStatus",
                    ErrorCodes.GatewayServiceFailed);
            }

            _logger.Debug("Application statuses (page {0}) were successfully retrieved from Legacy.GetApplicationsStatus ({1})",
                page, response.CandidateApplications.Count());

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications);
        }
    }
}
