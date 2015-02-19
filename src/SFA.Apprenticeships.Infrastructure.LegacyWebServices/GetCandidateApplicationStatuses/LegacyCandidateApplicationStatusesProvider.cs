namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.GetCandidateApplicationStatuses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.ApplicationUpdate;
    using Application.ApplicationUpdate.Entities;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using Wcf;

    using Candidate = Domain.Entities.Candidates.Candidate;

    public class LegacyCandidateApplicationStatusesProvider : ILegacyApplicationStatusesProvider
    {
        private readonly ILogService _logger;

        private readonly IMapper _mapper;
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyCandidateApplicationStatusesProvider(IWcfService<GatewayServiceContract> service, IMapper mapper, ILogService logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate)
        {
            try
            {
                _logger.Debug("Calling Legacy.GetCandidateInfo for candidate '{0}'", candidate.EntityId);

                var applicationStatuses = InternalGetCandidateApplicationStatuses(candidate);

                _logger.Debug("Candidate applications were successfully retrieved from Legacy.GetCandidateInfo ({0})", applicationStatuses.Count());

                return applicationStatuses;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                var de = new DomainException(ErrorCodes.GetCandidateInfoServiceFailed, e, new { candidateId = candidate.EntityId });

                _logger.Error(de);
                throw de;
            }
            catch (Exception e)
            {
                _logger.Error(e, new { candidateId = candidate.EntityId });
                throw;
            }
        }

        public int GetApplicationStatusesPageCount(int applicationStatusExtractWindow)
        {
            try
            {
                _logger.Debug("Calling Legacy.GetApplicationsStatus for page count");

                var totalPages = InternalGetApplicationStatusesPageCount(applicationStatusExtractWindow);

                _logger.Debug("Application statuses page count retrieved from Legacy.GetApplicationsStatus ({0})", totalPages);

                return totalPages;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                var de = new DomainException(ErrorCodes.GetApplicationsStatusServiceFailed, e, new { applicationStatusExtractWindow });

                _logger.Error(de);
                throw de;
            }
            catch (Exception e)
            {
                _logger.Error(e, new { applicationStatusExtractWindow });
                throw;
            }
        }

        public IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses(int pageNumber, int applicationStatusExtractWindow)
        {
            try
            {
                _logger.Debug("Calling Legacy.GetApplicationsStatus for page {0}", pageNumber);

                var applicationStatuses = InternalGetAllApplicationStatuses(pageNumber, applicationStatusExtractWindow);

                _logger.Debug("Application statuses (page {0}) were successfully retrieved from Legacy.GetApplicationsStatus ({1})", pageNumber, applicationStatuses.Count());

                return applicationStatuses;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                var de = new DomainException(ErrorCodes.GetApplicationsStatusServiceFailed, e, new { applicationStatusExtractWindow });

                _logger.Error(de);
                throw de;
            }
            catch (Exception e)
            {
                _logger.Error(e, new { applicationStatusExtractWindow });
                throw;
            }
        }

        #region Helpers

        private IList<ApplicationStatusSummary> InternalGetCandidateApplicationStatuses(Candidate candidate)
        {
            var request = new GetCandidateInfoRequest
            {
                CandidateId = candidate.LegacyCandidateId
            };

            var response = default(GetCandidateInfoResponse);

            _service.Use("SecureService", client => response = client.GetCandidateInfo(request).GetCandidateInfoResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                string message;

                if (response == null)
                {
                    message = "No response";
                }
                else
                {
                    message = string.Format("{0} validation error(s): {1}",
                        response.ValidationErrors.Count(), JsonConvert.SerializeObject(response, Formatting.None));
                }

                throw new DomainException(ErrorCodes.GetCandidateInfoServiceFailed, new { message, candidateId = candidate.EntityId, legacyCandidateId = candidate.LegacyCandidateId });
            }

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications).ToList();
        }

        private int InternalGetApplicationStatusesPageCount(int applicationStatusExtractWindow)
        {
            // retrieve application statuses page count so can queue subsequent paged requests
            var request = new GetApplicationsStatusRequest
            {
                PageNumber = 1,
                RangeTo = DateTime.UtcNow,
                RangeFrom = DateTime.UtcNow.AddMinutes(applicationStatusExtractWindow * -1)
            };

            var response = default(GetApplicationsStatusResponse);

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                throw new DomainException(ErrorCodes.GetApplicationsStatusServiceFailed,
                    new { pageNumber = request.PageNumber, applicationStatusExtractWindow, rangeFrom = request.RangeFrom, rangeTo = request.RangeTo });
            }

            return response.TotalPages;
        }

        private IList<ApplicationStatusSummary> InternalGetAllApplicationStatuses(int pageNumber, int applicationStatusExtractWindow)
        {
            // retrieve application statuses for ALL candidates (used in application ETL process)
            var request = new GetApplicationsStatusRequest
            {
                PageNumber = pageNumber,
                RangeTo = DateTime.UtcNow,
                RangeFrom = DateTime.UtcNow.AddMinutes(applicationStatusExtractWindow * -1)
            };

            var response = default(GetApplicationsStatusResponse);

            _service.Use("SecureService", client => response = client.GetApplicationsStatus(request));

            if (response == null)
            {
                throw new DomainException(ErrorCodes.GetApplicationsStatusServiceFailed,
                    new { pageNumber, applicationStatusExtractWindow, rangeFrom = request.RangeFrom, rangeTo = request.RangeTo });
            }

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications).ToList();
        }

        #endregion
    }
}
