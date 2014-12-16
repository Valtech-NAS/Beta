namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.GetCandidateApplicationStatuses
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using NLog;
    using Application.ApplicationUpdate;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class LegacyCandidateApplicationStatusesProvider : ILegacyApplicationStatusesProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyCandidateApplicationStatusesProvider
            (IWcfService<GatewayServiceContract> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate)
        {
            var request = new GetCandidateInfoRequest
            {
                CandidateId = candidate.LegacyCandidateId
            };

            var response = default(GetCandidateInfoResponse);

            Logger.Debug("Calling Legacy.GetCandidateInfo for candidate '{0}'", candidate.EntityId);

            _service.Use("SecureService", client => response = client.GetCandidateInfo(request).GetCandidateInfoResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    Logger.Error(
                        "Legacy.GetCandidateInfo reported {0} validation error(s): {1}",
                        response.ValidationErrors.Count(), responseAsJson);
                }
                else
                {
                    Logger.Error("Legacy.GetCandidateInfo did not respond");
                }

                var message =
                    string.Format("Failed to retrieve applications from Legacy.GetCandidateInfo for candidate '{0}'/'{1}'",
                        candidate.EntityId,
                        candidate.LegacyCandidateId);
                throw new CustomException(message, ErrorCodes.GatewayServiceFailed);
            }

            Logger.Debug("Candidate applications were successfully retrieved from Legacy.GetCandidateInfo ({0})",
                response.CandidateApplications.Count());

            return
                _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications);
        }

        public int GetApplicationStatusesPageCount()
        {
            // retrieve application statuses page count so can queue subsequent paged requests

            var request = new GetApplicationsStatusRequest
            {
                PageNumber = 1
            };

            var response = default(GetApplicationsStatusResponse);

            Logger.Debug("Calling Legacy.GetApplicationsStatus to find page count");

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                Logger.Error("Legacy.GetApplicationsStatus for page count did not respond");

                throw new CustomException("Failed to retrieve application status pages from Legacy.GetApplicationsStatus",
                    ErrorCodes.GatewayServiceFailed);
            }

            Logger.Debug("Application statuses were successfully retrieved from Legacy.GetApplicationsStatus ({0})", 
                response.TotalPages);

            return response.TotalPages;
        }

        public IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int pageNumber)
        {
            // retrieve application statuses for ALL candidates (used in application ETL process)

            var request = new GetApplicationsStatusRequest
            {
                PageNumber = pageNumber
            };

            var response = default(GetApplicationsStatusResponse);

            Logger.Debug("Calling Legacy.GetApplicationsStatus (page {0})", pageNumber);

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                Logger.Error("Legacy.GetApplicationsStatus did not respond");

                throw new CustomException("Failed to retrieve application statuses from Legacy.GetApplicationsStatus",
                    ErrorCodes.GatewayServiceFailed);
            }

            Logger.Debug("Application statuses (page {0}) were successfully retrieved from Legacy.GetApplicationsStatus ({1})",
                pageNumber, response.CandidateApplications.Count());

            return
                _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(
                    response.CandidateApplications);
        }
    }
}