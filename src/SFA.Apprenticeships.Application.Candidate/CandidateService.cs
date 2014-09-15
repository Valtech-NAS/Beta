namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Domain.Entities.Exceptions;
    using Strategies;
    using UserAccount.Strategies;

    public class CandidateService : ICandidateService
    {
        private readonly IActivateCandidateStrategy _activateCandidateStrategy;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IAuthenticateCandidateStrategy _authenticateCandidateStrategy;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICreateApplicationStrategy _createApplicationStrategy;
        private readonly IGetCandidateApplicationsStrategy _getCandidateApplicationsStrategy;
        private readonly IRegisterCandidateStrategy _registerCandidateStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISaveApplicationStrategy _saveApplicationStrategy;
        private readonly IArchiveApplicationStrategy _archiveApplicationStrategy;
        private readonly ISubmitApplicationStrategy _submitApplicationStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;
        private readonly IDeleteApplicationStrategy _deleteApplicationStrategy;

        public CandidateService(
            ICandidateReadRepository candidateReadRepository,
            ICandidateWriteRepository candidateWriteRepository,
            IApplicationReadRepository applicationReadRepository,
            IActivateCandidateStrategy activateCandidateStrategy,
            IAuthenticateCandidateStrategy authenticateCandidateStrategy,
            ISubmitApplicationStrategy submitApplicationStrategy,
            IRegisterCandidateStrategy registerCandidateStrategy,
            ICreateApplicationStrategy createApplicationStrategy,
            IGetCandidateApplicationsStrategy getCandidateApplicationsStrategy,
            IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            IUnlockAccountStrategy unlockAccountStrategy,
            ISaveApplicationStrategy saveApplicationStrategy,
            IArchiveApplicationStrategy archiveApplicationStrategy, 
            IDeleteApplicationStrategy deleteApplicationStrategy)
        {
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
            _archiveApplicationStrategy = archiveApplicationStrategy;
            _deleteApplicationStrategy = deleteApplicationStrategy;
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

            try
            {

                _activateCandidateStrategy.ActivateCandidate(username, activationCode);
            }
            catch ( CustomException )
            {
                var message = string.Format("Activate user failed for user {0}", username);
                throw new CustomException(message, Interfaces.Candidates.ErrorCodes.ActivateUserInvalidCode);
            }
            catch (Exception )
            {
                var message = string.Format("Activate user failed for user {0}", username);
                throw new CustomException(message, Interfaces.Candidates.ErrorCodes.ActivateUserFailed);
            }
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

        public Candidate GetCandidate(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            return _candidateReadRepository.Get(username);
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

        public ApplicationDetail GetApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            var applicationId = GetApplicationId(candidateId, vacancyId);

            return _applicationReadRepository.Get(applicationId);
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            var applicationId = GetApplicationId(candidateId, vacancyId);

            _archiveApplicationStrategy.ArchiveApplication(applicationId);
        }

        public void DeleteApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            var applicationId = GetApplicationId(candidateId, vacancyId);
            _deleteApplicationStrategy.DeleteApplication(applicationId);
        }

        public void SaveApplication(Guid candidateId, int vacancyId, ApplicationDetail application)
        {
            Condition.Requires(application);

            var applicationId = GetApplicationId(candidateId, vacancyId);
            application.EntityId = applicationId;

            _saveApplicationStrategy.SaveApplication(application);
        }

        public IList<ApplicationSummary> GetApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            return _getCandidateApplicationsStrategy.GetApplications(candidateId);
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            var applicationId = GetApplicationId(candidateId, vacancyId);

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

        private Guid GetApplicationId(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _applicationReadRepository
                .GetForCandidate(candidateId, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            return applicationDetail.EntityId;
        }
    }
}
