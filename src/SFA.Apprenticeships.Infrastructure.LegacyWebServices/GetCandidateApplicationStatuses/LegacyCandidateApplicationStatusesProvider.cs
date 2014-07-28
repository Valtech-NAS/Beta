﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.GetCandidateApplicationStatuses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Candidate.Strategies;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;

    public class LegacyCandidateApplicationStatusesProvider : ILegacyApplicationStatusesProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<GatewayServiceContract> _service;
        private readonly IMapper _mapper;

        public LegacyCandidateApplicationStatusesProvider(IWcfService<GatewayServiceContract> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public IEnumerable<ApplicationStatusSummary> GetCandidateApplicationStatuses(Candidate candidate)
        {
            Logger.Info("GetCandidateApplications handled for EntityId={0}, EmailAddress={1}", candidate.EntityId, candidate.RegistrationDetails.EmailAddress);
            var request = new GetCandidateInfoRequest
            {
                CandidateId = candidate.LegacyCandidateId
            };

            var response = default(GetCandidateInfoResponse);
            _service.Use(client => response = client.GetCandidateInfo(request).GetCandidateInfoResponse);

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    Logger.Error("Legacy GetCandidateApplications reported {0} validation errors", response.ValidationErrors.Count());
                }
                else
                {
                    Logger.Error("Legacy GetCandidateApplications did not respond");
                }
                // TODO: EXCEPTION: should use an application exception type
                throw new Exception("Failed to retrieve candidate applications in legacy system");
            }

            Logger.Info("Candidate applications were successfully retrieved from Legacy web service ({0})", response.CandidateApplications.Count());

            return _mapper.Map<CandidateApplication[], IEnumerable<ApplicationStatusSummary>>(response.CandidateApplications);
        }

        public IEnumerable<ApplicationStatusSummary> GetAllApplicationStatuses()
        {
            //todo: retrieve application statuses for ALL candidates (used in ETL process)
            throw new NotImplementedException();
        }
    }
}