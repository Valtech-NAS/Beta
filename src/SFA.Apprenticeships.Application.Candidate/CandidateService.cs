namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Interfaces.Users;
    using Strategies;

    public class CandidateService : ICandidateService
    {
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IActivateCandidateStrategy _activateCandidateStrategy;
        private readonly ISubmitApplicationStrategy _submitApplicationStrategy;
        private readonly IRegisterCandidateStrategy _registerCandidateStrategy;

        public CandidateService(IApplicationWriteRepository applicationWriteRepository, ICandidateReadRepository candidateReadRepository, ICandidateWriteRepository candidateWriteRepository, IAuthenticationService authenticationService, IUserReadRepository userReadRepository, IActivateCandidateStrategy activateCandidateStrategy, ISubmitApplicationStrategy submitApplicationStrategy, IRegisterCandidateStrategy registerCandidateStrategy)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _authenticationService = authenticationService;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _userReadRepository = userReadRepository;
            _activateCandidateStrategy = activateCandidateStrategy;
            _submitApplicationStrategy = submitApplicationStrategy;
            _registerCandidateStrategy = registerCandidateStrategy;
        }

        public Candidate RegisterCandidate(Candidate newCandidate, string password)
        {
            Condition.Requires(newCandidate);
            Condition.Requires(password).IsNotNullOrEmpty();

            return _registerCandidateStrategy.RegisterCandidate(newCandidate, password);
        }

        public void ActivateCandidate(string username, string activationCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _activateCandidateStrategy.ActivateCandidate(username, activationCode);
        }

        public Candidate Authenticate(string username, string password)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(password).IsNotNullOrEmpty();

            var user = _userReadRepository.Get(username);

            //todo: check status of user (may not be active)
            //todo: check role of user? (may not be a candidate ... User.Roles)

            _authenticationService.AuthenticateUser(user.EntityId, password);

            var candidate = _candidateReadRepository.Get(user.EntityId);

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

            //todo: ensure candidate has not already applied for the vacancy

            _submitApplicationStrategy.SubmitApplication(application);
        }
    }
}
