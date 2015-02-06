namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;

    /// <summary>
    /// For candidate users to register, manage their profile and other dashboard entities
    /// </summary>
    public interface ICandidateService
    {
        Candidate Register(Candidate newCandidate, string password);

        void Activate(string username, string activationCode);

        Candidate Authenticate(string username, string password);

        Candidate GetCandidate(Guid id);

        Candidate GetCandidate(string username);

        Candidate SaveCandidate(Candidate candidate);

        ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId); // note: only an int due to legacy - will be a Guid

        ApprenticeshipApplicationDetail GetApplication(Guid candidateId, int vacancyId);

        void ArchiveApplication(Guid candidateId, int vacancyId);

        void UnarchiveApplication(Guid candidateId, int vacancyId);

        void SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication);

        IList<ApprenticeshipApplicationSummary> GetApprenticeshipApplications(Guid candidateId);

        void SubmitApplication(Guid candidateId, int vacancyId);

        TraineeshipApplicationDetail CreateTraineeshipApplication(Guid candidateId, int traineeshipVacancyId);

        TraineeshipApplicationDetail GetTraineeshipApplication(Guid candidateId, int vacancyId);

        IList<TraineeshipApplicationSummary> GetTraineeshipApplications(Guid candidateId);

        void SubmitTraineeshipApplication(Guid candidateId, int vacancyId, TraineeshipApplicationDetail traineeshipApplicationDetail);

        void UnlockAccount(string username, string accountUnlockCode);

        void ResetForgottenPassword(string username, string passwordCode, string newPassword);

        void DeleteApplication(Guid candidateId, int vacancyId);

        ApprenticeshipVacancyDetail GetApprenticeshipVacancyDetail(Guid candidateId, int vacancyId);

        TraineeshipVacancyDetail GetTraineeshipVacancyDetail(Guid candidateId, int vacancyId);

        //todo: 1.6: void SendMobileVerificationCode(string username);

        //todo: 1.6: void VerifyMobilecode(string username, string verificationCode);
    }
}
