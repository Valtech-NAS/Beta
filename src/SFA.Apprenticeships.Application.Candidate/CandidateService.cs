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
    using NLog;
    using Strategies;
    using UserAccount.Strategies;

    public class CandidateService : ICandidateService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IActivateCandidateStrategy _activateCandidateStrategy;
        private readonly IApplicationReadRepository _applicationReadRepository;
        private readonly IAuthenticateCandidateStrategy _authenticateCandidateStrategy;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICreateApplicationStrategy _createApplicationStrategy;
        private readonly IGetCandidateApplicationsStrategy _getCandidateApplicationsStrategy;
        private readonly IRegisterCandidateStrategy _registerCandidateStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISaveApplicationStrategy _saveApplicationStrategy;
        private readonly IArchiveApplicationStrategy _archiveApplicationStrategy;
        private readonly ISubmitApplicationStrategy _submitApplicationStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;
        private readonly IDeleteApplicationStrategy _deleteApplicationStrategy;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;

        public CandidateService(
            ICandidateReadRepository candidateReadRepository,
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
            IDeleteApplicationStrategy deleteApplicationStrategy, 
            ISaveCandidateStrategy saveCandidateStrategy)
        {
            _candidateReadRepository = candidateReadRepository;
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
            _saveCandidateStrategy = saveCandidateStrategy;
        }

        public Candidate Register(Candidate newCandidate, string password)
        {
            Condition.Requires(newCandidate);
            Condition.Requires(password).IsNotNullOrEmpty();

            Logger.Debug("Calling CandidateService to register a new candidate.");

            var candidate = _registerCandidateStrategy.RegisterCandidate(newCandidate, password);

            return candidate;
        }

        public void Activate(string username, string activationCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(activationCode).IsNotNullOrEmpty();

            Logger.Debug("Calling CandidateService to activate the user {0}.", username);

            //todo: this error "handling" block shouldn't be here
            try
            {
                _activateCandidateStrategy.ActivateCandidate(username, activationCode);
            }
            catch (CustomException e)
            {
                var message = string.Format("Activate user failed for user {0}", username);
                Logger.Debug(message, e);
                throw new CustomException(message, Interfaces.Candidates.ErrorCodes.ActivateUserInvalidCode);
            }
            catch (Exception e)
            {
                var message = string.Format("Activate user failed for user {0}", username);
                Logger.Debug(message, e);
                throw new CustomException(message, Interfaces.Candidates.ErrorCodes.ActivateUserFailed);
            }
        }

        public Candidate Authenticate(string username, string password)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(password).IsNotNullOrEmpty();

            Logger.Debug("Calling CandidateService to authenticate the user {0}.", username);

            return _authenticateCandidateStrategy.AuthenticateCandidate(username, password);
        }

        public Candidate GetCandidate(Guid id)
        {
            Logger.Debug("Calling CandidateService to get the user with Id={0}.", id);
            return _candidateReadRepository.Get(id);
        }

        public Candidate GetCandidate(string username)
        {
            Condition.Requires(username).IsNotNullOrEmpty();

            Logger.Debug("Calling CandidateService to get the user {0}.", username);

            return _candidateReadRepository.Get(username);
        }

        public Candidate SaveCandidate(Candidate candidate)
        {
            Condition.Requires(candidate);

            Logger.Debug("Calling CandidateService to save a candidate.");

            return _saveCandidateStrategy.SaveCandidate(candidate);
        }

        public void UnlockAccount(string username, string accountUnlockCode)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(accountUnlockCode).IsNotNullOrEmpty();

            Logger.Debug("Calling CandidateService to unlock the account of the user {0}.", username);

            _unlockAccountStrategy.UnlockAccount(username, accountUnlockCode);
        }

        public void ResetForgottenPassword(string username, string passwordCode, string newPassword)
        {
            Condition.Requires(username).IsNotNullOrEmpty();
            Condition.Requires(passwordCode).IsNotNullOrEmpty();
            Condition.Requires(newPassword).IsNotNullOrEmpty();

            Logger.Debug("Calling CandidateService to reseth the password for the user {0}.", username);

            _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
        }

        public ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to create an apprenticeshipApplication of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            return _createApplicationStrategy.CreateApplication(candidateId, vacancyId);
        }

        public ApprenticeshipApplicationDetail GetApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to get the apprenticeshipApplication of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            var applicationId = GetApplicationId(candidateId, vacancyId);

            return _applicationReadRepository.Get(applicationId);
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to archive the apprenticeshipApplication of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            var applicationId = GetApplicationId(candidateId, vacancyId);

            _archiveApplicationStrategy.ArchiveApplication(applicationId);
        }

        public void DeleteApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to delete the apprenticeshipApplication of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            var applicationId = GetApplicationId(candidateId, vacancyId);
            _deleteApplicationStrategy.DeleteApplication(applicationId);
        }

        public void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            Condition.Requires(apprenticeshipApplication);

            Logger.Debug(
                "Calling CandidateService to save the apprenticeshipApplication of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            var applicationId = GetApplicationId(candidateId, vacancyId);
            apprenticeshipApplication.EntityId = applicationId;

            _saveApplicationStrategy.SaveApplication(apprenticeshipApplication);
        }

        public IList<ApplicationSummary> GetApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to get the applications of the user with Id={0}.",
                candidateId);

            return _getCandidateApplicationsStrategy.GetApplications(candidateId);
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to submit the apprenticeshipApplication of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            var applicationId = GetApplicationId(candidateId, vacancyId);

            _submitApplicationStrategy.SubmitApplication(applicationId);
        }

        private Guid GetApplicationId(Guid candidateId, int vacancyId)
        {
            var applicationDetail = _applicationReadRepository
                .GetForCandidate(candidateId, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            return applicationDetail.EntityId;
        }
    }
}
