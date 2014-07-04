namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Interfaces.Users;

    public class CandidateService : ICandidateService
    {
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IRegistrationService _registrationService;
        private readonly IUserReadRepository _userReadRepository;
        //todo: private readonly IApplicationSubmissionQueue _applicationSubmissionQueue;

        public CandidateService(IApplicationWriteRepository applicationWriteRepository, ICandidateReadRepository candidateReadRepository, ICandidateWriteRepository candidateWriteRepository, IAuthenticationService authenticationService, IRegistrationService registrationService, IUserReadRepository userReadRepository)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _authenticationService = authenticationService;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _registrationService = registrationService;
            _userReadRepository = userReadRepository;
            //todo: _applicationSubmissionQueue = applicationSubmissionQueue
        }

        public bool IsUsernameAvailable(string username)
        {
            return _registrationService.IsUsernameAvailable(username);
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            var username = newCandidate.EmailAddress;
            var newCandidateId = Guid.NewGuid();
            var activationCode = "TODO"; //todo: generate a unique activation code (ICodeProvider)

            newCandidate.Id = newCandidateId;

            _registrationService.Register(username, newCandidateId, activationCode);

            _authenticationService.CreateUser(newCandidateId, password);

            var candidate = _candidateWriteRepository.Save(newCandidate);

            var legacyCandidateId = 123; //todo: send candidate to legacy

            return candidate;
        }

        public Candidate Authenticate(string username, string password)
        {
            var user = _userReadRepository.Get(username);

            //todo: check status of user (may not be active)
            //todo: check role of user? (may not be a candidate ... User.Roles)

            _authenticationService.AuthenticateUser(user.Id, password);

            var candidate = _candidateReadRepository.Get(user.Id);

            return candidate;
        }

        public Candidate GetCandidate(Guid id)
        {
            return _candidateReadRepository.Get(id);
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            Condition.Requires(candidate);

            return _candidateWriteRepository.Save(candidate);
        }

        public ApplicationDetail SaveApplication(ApplicationDetail application)
        {
            Condition.Requires(application);

            return _applicationWriteRepository.Save(application);
        }

        public void SubmitApplication(ApplicationDetail application)
        {
            Condition.Requires(application);

            //todo: _applicationSubmissionQueue.Queue(application);
            throw new NotImplementedException();
        }
    }
}
