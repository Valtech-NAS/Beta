namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Domain.Entities.Exceptions;
    using Interfaces.Logging;
    using Strategies;
    using Strategies.Apprenticeships;
    using Strategies.Traineeships;
    using UserAccount.Strategies;
    using ErrorCodes = Interfaces.Candidates.ErrorCodes;

    public class CandidateService : ICandidateService
    {
        private readonly ILogService Logger;

        private readonly IActivateCandidateStrategy _activateCandidateStrategy;
        private readonly IApprenticeshipApplicationReadRepository _apprenticeshipApplicationReadRepository;
        private readonly ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private readonly IAuthenticateCandidateStrategy _authenticateCandidateStrategy;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICreateApprenticeshipApplicationStrategy _createApplicationStrategy;
        private readonly ICreateTraineeshipApplicationStrategy _createTraineeshipApplicationStrategy;
        private readonly IGetCandidateApprenticeshipApplicationsStrategy _getCandidateApprenticeshipApplicationsStrategy;
        private readonly IGetCandidateTraineeshipApplicationsStrategy _getCandidateTraineeshipApplicationsStrategy;
        private readonly IRegisterCandidateStrategy _registerCandidateStrategy;
        private readonly IResetForgottenPasswordStrategy _resetForgottenPasswordStrategy;
        private readonly ISaveApprenticeshipApplicationStrategy _saveApplicationStrategy;
        private readonly ISaveTraineeshipApplicationStrategy _saveTraineeshipApplicationStrategy;
        private readonly IArchiveApplicationStrategy _archiveApplicationStrategy;
        private readonly ISubmitApprenticeshipApplicationStrategy _submitApprenticeshipApplicationStrategy;
        private readonly ISubmitTraineeshipApplicationStrategy _submitTraineeshipApplicationStrategy;
        private readonly IUnlockAccountStrategy _unlockAccountStrategy;
        private readonly IDeleteApplicationStrategy _deleteApplicationStrategy;
        private readonly ISaveCandidateStrategy _saveCandidateStrategy;
        private readonly ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail> _candidateApprenticeshipVacancyDetailStrategy;
        private readonly ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail> _candidateTraineeshipVacancyDetailStrategy;

        public CandidateService(
            ICandidateReadRepository candidateReadRepository,
            IApprenticeshipApplicationReadRepository apprenticeshipApplicationReadRepository,
            IActivateCandidateStrategy activateCandidateStrategy,
            IAuthenticateCandidateStrategy authenticateCandidateStrategy,
            ISubmitApprenticeshipApplicationStrategy submitApprenticeshipApplicationStrategy,
            IRegisterCandidateStrategy registerCandidateStrategy,
            ICreateApprenticeshipApplicationStrategy createApplicationStrategy,
            ICreateTraineeshipApplicationStrategy createTraineeshipApplicationStrategy,
            IGetCandidateApprenticeshipApplicationsStrategy getCandidateApprenticeshipApplicationsStrategy,
            IResetForgottenPasswordStrategy resetForgottenPasswordStrategy,
            IUnlockAccountStrategy unlockAccountStrategy,
            ISaveApprenticeshipApplicationStrategy saveApplicationStrategy,
            IArchiveApplicationStrategy archiveApplicationStrategy, 
            IDeleteApplicationStrategy deleteApplicationStrategy, 
            ISaveCandidateStrategy saveCandidateStrategy,
            ISubmitTraineeshipApplicationStrategy submitTraineeshipApplicationStrategy, 
            ISaveTraineeshipApplicationStrategy saveTraineeshipApplicationStrategy, 
            ITraineeshipApplicationReadRepository traineeshipApplicationReadRepository,
            IGetCandidateTraineeshipApplicationsStrategy getCandidateTraineeshipApplicationsStrategy,
            ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail> candidateApprenticeshipVacancyDetailStrategy,
            ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail> candidateTraineeshipVacancyDetailStrategy, ILogService logService)
        {
            _candidateReadRepository = candidateReadRepository;
            _activateCandidateStrategy = activateCandidateStrategy;
            _authenticateCandidateStrategy = authenticateCandidateStrategy;
            _submitApprenticeshipApplicationStrategy = submitApprenticeshipApplicationStrategy;
            _registerCandidateStrategy = registerCandidateStrategy;
            _createApplicationStrategy = createApplicationStrategy;
            _createTraineeshipApplicationStrategy = createTraineeshipApplicationStrategy;
            _getCandidateApprenticeshipApplicationsStrategy = getCandidateApprenticeshipApplicationsStrategy;
            _resetForgottenPasswordStrategy = resetForgottenPasswordStrategy;
            _unlockAccountStrategy = unlockAccountStrategy;
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            _saveApplicationStrategy = saveApplicationStrategy;
            _archiveApplicationStrategy = archiveApplicationStrategy;
            _deleteApplicationStrategy = deleteApplicationStrategy;
            _saveCandidateStrategy = saveCandidateStrategy;
            _submitTraineeshipApplicationStrategy = submitTraineeshipApplicationStrategy;
            _saveTraineeshipApplicationStrategy = saveTraineeshipApplicationStrategy;
            _traineeshipApplicationReadRepository = traineeshipApplicationReadRepository;
            _getCandidateTraineeshipApplicationsStrategy = getCandidateTraineeshipApplicationsStrategy;
            _candidateApprenticeshipVacancyDetailStrategy = candidateApprenticeshipVacancyDetailStrategy;
            _candidateTraineeshipVacancyDetailStrategy = candidateTraineeshipVacancyDetailStrategy;
            Logger = logService;
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

            _activateCandidateStrategy.ActivateCandidate(username, activationCode);
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

            Logger.Debug("Calling CandidateService to reset the password for the user {0}.", username);

            _resetForgottenPasswordStrategy.ResetForgottenPassword(username, passwordCode, newPassword);
        }

        public ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to create an apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            return _createApplicationStrategy.CreateApplication(candidateId, vacancyId);
        }

        public ApprenticeshipApplicationDetail GetApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to get the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            return _apprenticeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);
        }

        public TraineeshipApplicationDetail CreateTraineeshipApplication(Guid candidateId, int traineeshipVacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to create a traineeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, traineeshipVacancyId);

            return _createTraineeshipApplicationStrategy.CreateApplication(candidateId, traineeshipVacancyId);
        }

        public void ArchiveApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to archive the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            _archiveApplicationStrategy.ArchiveApplication(candidateId, vacancyId);
        }

        public void UnarchiveApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to unarchive the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            _archiveApplicationStrategy.UnarchiveApplication(candidateId, vacancyId);
        }

        public void DeleteApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to delete the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            _deleteApplicationStrategy.DeleteApplication(candidateId, vacancyId);
        }

        public TraineeshipApplicationDetail GetTraineeshipApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to get the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            return _traineeshipApplicationReadRepository.GetForCandidate(candidateId, vacancyId);
        }

        public void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication)
        {
            Condition.Requires(apprenticeshipApplication);

            Logger.Debug(
                "Calling CandidateService to save the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            _saveApplicationStrategy.SaveApplication(candidateId, vacancyId, apprenticeshipApplication);
        }

        public IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to get the apprenticeship applications of the user with Id={0}.",
                candidateId);

            return _getCandidateApprenticeshipApplicationsStrategy.GetApplications(candidateId);
        }

        public void SubmitApplication(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to submit the apprenticeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            _submitApprenticeshipApplicationStrategy.SubmitApplication(candidateId, vacancyId);
        }

        public void SubmitTraineeshipApplication(Guid candidateId, int vacancyId,
            TraineeshipApplicationDetail traineeshipApplicationDetail)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to submit the traineeship application of the user with Id={0} to the apprenticeshipApplication with Id={1}.",
                candidateId, vacancyId);

            var traineeshipDetails = _saveTraineeshipApplicationStrategy.SaveApplication(traineeshipApplicationDetail);
            _submitTraineeshipApplicationStrategy.SubmitApplication(traineeshipDetails.EntityId);
        }

        public IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId)
        {
            Condition.Requires(candidateId);

            Logger.Debug(
                "Calling CandidateService to get the traineeship applications of the user with Id={0}.",
                candidateId);

            return _getCandidateTraineeshipApplicationsStrategy.GetApplications(candidateId);
        }

        public ApprenticeshipVacancyDetail GetApprenticeshipVacancyDetail(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId).IsGreaterOrEqual(0);

            Logger.Debug("Calling CandidateService to get the apprenticeship vacancy ID {0} for candidaqte ID {1}.", vacancyId, candidateId);

            return _candidateApprenticeshipVacancyDetailStrategy.GetVacancyDetails(candidateId, vacancyId);
        }

        public TraineeshipVacancyDetail GetTraineeshipVacancyDetail(Guid candidateId, int vacancyId)
        {
            Condition.Requires(candidateId);
            Condition.Requires(vacancyId).IsGreaterOrEqual(0);

            Logger.Debug("Calling CandidateService to get the traineeship vacancy ID {0} for candidaqte ID {1}.", vacancyId, candidateId);

            return _candidateTraineeshipVacancyDetailStrategy.GetVacancyDetails(candidateId, vacancyId);
        }
    }
}
