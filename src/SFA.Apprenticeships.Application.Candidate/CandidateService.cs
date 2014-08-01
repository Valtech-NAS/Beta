namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Strategies;
    using UserAccount.Strategies;

    public class CandidateService : ICandidateService
    {
        private readonly IActivateCandidateStrategy _activateCandidateStrategy;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IApplicationWriteRepository _applicationWriteRepository;
        private readonly IAuthenticateCandidateStrategy _authenticateCandidateStrategy;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICreateApplicationStrategy _createApplicationStrategy;
        private readonly IGetCandidateApplicationsStrategy _getCandidateApplicationsStrategy;
        private readonly IRegisterCandidateStrategy _registerCandidateStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISaveApplicationStrategy _saveApplicationStrategy;
        private readonly ISubmitApplicationStrategy _submitApplicationStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;

        public CandidateService(IApplicationWriteRepository applicationWriteRepository,
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            IActivateCandidateStrategy activateCandidateStrategy,
            IAuthenticateCandidateStrategy authenticateCandidateStrategy,
            ISubmitApplicationStrategy submitApplicationStrategy,
            IRegisterCandidateStrategy registerCandidateStrategy,
            ICreateApplicationStrategy createApplicationStrategy,
            IGetCandidateApplicationsStrategy getCandidateApplicationsStrategy,
            IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            IUnlockAccountStrategy unlockAccountStrategy, IApplicationReadRepository applicationReadRepository,
            ISaveApplicationStrategy saveApplicationStrategy)
        {
            _applicationWriteRepository = applicationWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _activateCandidateStrategy = activateCandidateStrategy;
            _authenticateCandidateStrategy = authenticateCandidateStrategy;
            _submitApplicationStrategy = submitApplicationStrategy;
            _registerCandidateStrategy = registerCandidateStrategy;
            _createApplicationStrategy = createApplicationStrategy;
            _getCandidateApplicationsStrategy = getCandidateApplicationsStrategy;
            _resetForgottenPasswordStrategy = resetForgottenPasswordStrategy;
            _unlockAccountStrategy = unlockAccountStrategy;
            _applicationReadRepository = applicationReadRepository;
            _saveApplicationStrategy = saveApplicationStrategy;
        }

        public Candidate Register(Candidate newCandidate, string password)
        {
            Condition.Requires(newCandidate);
            Condition.Requires(password).IsNotNullOrEmpty();

            var candidate = _registerCandidateStrategy.RegisterCandidate(newCandidate, password);

            return candidate;
        }

        public void Activate(string username, string activationCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            _activateCandidateStrategy.ActivateCandidate(username, activationCode);
        }

        public Candidate Authenticate(string username, string password)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(password).IsNotNullOrEmpty();

            return _authenticateCandidateStrategy.AuthenticateCandidate(username, password);
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

        public ApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            return _createApplicationStrategy.CreateApplication(candidateId, vacancyId);
        }

        public ApplicationDetail GetApplication(Guid applicationId)
        {
            Condition.Requires(applicationId);

            return _applicationReadRepository.Get(applicationId);
        }

        public ApplicationDetail SaveApplication(ApplicationDetail application)
        {
            Condition.Requires(application);

            return _saveApplicationStrategy.SaveApplication(application);
        }

        public IList<ApplicationSummary> GetApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            return _getCandidateApplicationsStrategy.GetApplications(candidateId);
        }

        public void SubmitApplication(Guid applicationId)
        {
            Condition.Requires(applicationId);

            _submitApplicationStrategy.SubmitApplication(applicationId);
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(accountUnlockCode).IsNotNullOrEmpty();

            _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(passwordCode).IsNotNullOrEmpty();
            Condition.Requires(newPassword).IsNotNullOrEmpty();

            _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
        }
    }
}