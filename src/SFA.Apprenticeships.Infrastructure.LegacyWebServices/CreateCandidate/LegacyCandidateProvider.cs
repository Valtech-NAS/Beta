namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.CreateCandidate
{
    using System;
    using System.Linq;
    using Application.Candidate.Strategies;
    using GatewayServiceProxy;
    using NLog;
    using Wcf;
    using Candidate = Domain.Entities.Candidates.Candidate;
    using Newtonsoft.Json;

    public class LegacyCandidateProvider : ILegacyCandidateProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyCandidateProvider(IWcfService<GatewayServiceContract> service)
        {
            _service = service;
        }

        public int CreateCandidate(Candidate candidate)
        {
            Logger.Debug("CreateCandidate handled for EntityId={0}, EmailAddress={1}", candidate.EntityId, candidate.RegistrationDetails.EmailAddress);
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
            _service.Use("SecureService", client => response = client.CreateCandidate(request));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                if (response != null)
                {
                    var responseAsJson = JsonConvert.SerializeObject(response, Formatting.None);

                    Logger.Error("Legacy CreateCandidate reported {0} validation error(s): {1}", response.ValidationErrors.Count(), responseAsJson);
                }
                else
                {
                    Logger.Error("Legacy CreateCandidate did not respond");
                }
                // TODO: EXCEPTION: should use an application exception type
                throw new Exception("Failed to create candidate in legacy system");
            }

            var legacyCandidateId = response.CandidateId;

            Logger.Debug("Candidate was successfully created on Legacy web service. LegacyCandidateId={0} ", legacyCandidateId);

            return legacyCandidateId;
        }
    }
}
