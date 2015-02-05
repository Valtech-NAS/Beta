namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateCandidate
{
    using System;
    using System.Linq;
    using Application.Candidate;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;
    using CreateCandidateRequest = GatewayServiceProxy.CreateCandidateRequest;
    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;

    public class LegacyCandidateProvider : ILegacyCandidateProvider
    {
        private readonly ILogService _logger;
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyCandidateProvider(IWcfService<GatewayServiceContract> service, ILogService logger)
        {
            _service = service;
            _logger = logger;
        }

        public int CreateCandidate(Candidate candidate)
        {
            var request = new CreateCandidateRequest
            {
                Candidate = new GatewayServiceProxy.Candidate
                {
                    EmailAddress = candidate.RegistrationDetails.EmailAddress,
                    FirstName = candidate.RegistrationDetails.FirstName,
                    MiddleName = candidate.RegistrationDetails.MiddleNames,
                    Surname = candidate.RegistrationDetails.LastName,
                    DateOfBirth = candidate.RegistrationDetails.DateOfBirth.Date,
                    AddressLine1 = candidate.RegistrationDetails.Address.AddressLine1,
                    AddressLine2 = candidate.RegistrationDetails.Address.AddressLine2,
                    AddressLine3 = candidate.RegistrationDetails.Address.AddressLine3,
                    AddressLine4 = candidate.RegistrationDetails.Address.AddressLine4,
                    TownCity = "N/A",
                    Postcode = candidate.RegistrationDetails.Address.Postcode,
                    LandlineTelephone = candidate.RegistrationDetails.PhoneNumber,
                    MobileTelephone = string.Empty
                }
            };

            var response = default(CreateCandidateResponse);

            _logger.Debug("Calling Legacy.CreateCandidate for candidate '{0}'", candidate.EntityId);

            _service.Use("SecureService", client => response = client.CreateCandidate(request));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    _logger.Error("Legacy.CreateCandidate reported {0} validation error(s): {1} for NAS candidate id: {2}", 
                        response.ValidationErrors.Count(), 
                        responseAsJson,
                        candidate.EntityId);
                }
                else
                {
                    _logger.Error("Legacy.CreateCandidate did not respond");
                }

                throw new CustomException("Failed to create candidate in Legacy.CreateCandidate", CandidateErrorCodes.CandidateCreationError);
            }

            var legacyCandidateId = response.CandidateId;

            _logger.Debug("Candidate created in Legacy.CreateCandidate (candidate '{0}'/'{1}')", candidate.EntityId, legacyCandidateId);

            return legacyCandidateId;
        }
    }
}
