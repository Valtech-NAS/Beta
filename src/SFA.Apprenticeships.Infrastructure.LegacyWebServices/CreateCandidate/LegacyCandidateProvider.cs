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

    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;
    using CreateCandidateRequest = GatewayServiceProxy.CreateCandidateRequest;

    public class LegacyCandidateProvider : ILegacyCandidateProvider
    {
        private readonly ILogService _logger;
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyCandidateProvider(IWcfService<GatewayServiceContract> service, ILogService logger)
        {
            _service = service;
            _logger = logger;
        }

        public int CreateCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            try
            {
                _logger.Debug("Calling Legacy.CreateCandidate for candidate id='{0}'", candidate.EntityId);

                var legacyCandidateId = InternalCreateCandidate(candidate);

                _logger.Debug("Legacy.CreateCandidate succeeded for candidate id='{0}', legacy candidate id='{1}'", candidate.EntityId, legacyCandidateId);

                return legacyCandidateId;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                var de = new DomainException(CandidateErrorCodes.CreateCandiateFailed, e, new { candidateId = candidate.EntityId });

                _logger.Error(de);
                throw de;
            }
            catch (Exception e)
            {
                _logger.Error(e, new { candidateId = candidate.EntityId });
                throw;
            }
        }

        private int InternalCreateCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            var request = CreateRequest(candidate);
            var response = default(CreateCandidateResponse);

            _service.Use("SecureService", client => response = client.CreateCandidate(request));

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

                throw new DomainException(CandidateErrorCodes.CreateCandiateFailed, new { message, candidateId = candidate.EntityId });
            }

            return response.CandidateId;
        }

        private static CreateCandidateRequest CreateRequest(Domain.Entities.Candidates.Candidate candidate)
        {
            return new CreateCandidateRequest
            {
                Candidate = new Candidate
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
        }
    }
}
