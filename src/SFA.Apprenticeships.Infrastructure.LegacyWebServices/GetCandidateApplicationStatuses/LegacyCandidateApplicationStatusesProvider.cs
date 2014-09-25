namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.GetCandidateApplicationStatuses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.ApplicationUpdate;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using NLog;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;
    using ErrorCodes = Application.VacancyEtl.ErrorCodes;

    public class LegacyCandidateApplicationStatusesProvider : ILegacyApplicationStatusesProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyCandidateApplicationStatusesProvider
            (IWcfService<GatewayServiceContract> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate)
        {
            Logger.Debug("GetCandidateApplications handled for EntityId={0}, EmailAddress={1}", candidate.EntityId, candidate.RegistrationDetails.EmailAddress);

            var request = new GetCandidateInfoRequest
            {
                CandidateId = candidate.LegacyCandidateId
            };

            var response = default(GetCandidateInfoResponse);

            _service.Use("DefaultEndpoint", client => response = client.GetCandidateInfo(request).GetCandidateInfoResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    Logger.Error(
                        "Legacy GetCandidateInfo reported {0} validation error(s): {1}",
                        response.ValidationErrors.Count(), responseAsJson);
                }
                else
                {
                    Logger.Error("Legacy GetCandidateInfo did not respond");
                }

                // TODO: EXCEPTION: should use an application exception type
                var message =
                    string.Format("Failed to retrieve candidate applications in legacy system for candidate {0}",
                        candidate.LegacyCandidateId);
                throw new Exception(message);
            }

            Logger.Debug("Candidate applications were successfully retrieved from Legacy web service ({0})", response.CandidateApplications.Count());

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications);
        }

        public int GetApplicationStatusesPageCount()
        {
            // retrieve application statuses page count so can queue subsequent paged requests

            Logger.Debug("Calling GetAllApplicationStatuses to find page count");

            var request = new GetApplicationsStatusRequest
            {
                PageNumber = 1
            };

            var response = default(GetApplicationsStatusResponse);

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                Logger.Error("Legacy GetAllApplicationStatuses for page count did not respond");

                throw new CustomException("Failed to retrieve application statuses page count in legacy system", ErrorCodes.GatewayServiceFailed);
            }

            Logger.Debug("Application statuses page count returned {0}", response.TotalPages);

            return response.TotalPages;
        }

        public IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int pageNumber)
        {
            // retrieve application statuses for ALL candidates (used in application ETL process)

            Logger.Debug("Calling GetAllApplicationStatuses (page {0})", pageNumber);

            var request = new GetApplicationsStatusRequest
            {
                PageNumber = pageNumber
            };

            var response = default(GetApplicationsStatusResponse);

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                Logger.Error("Legacy GetAllApplicationStatuses did not respond");

                throw new CustomException("Failed to retrieve application statuses in legacy system", ErrorCodes.GatewayServiceFailed);
            }

            Logger.Debug("Application statuses (page {0}) were successfully retrieved from Legacy web service ({1})", 
                pageNumber, response.CandidateApplications.Count());

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications);
        }
    }
}
