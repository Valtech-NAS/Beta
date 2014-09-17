namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

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

        ApplicationDetail CreateApplication(Guid candidateId, int vacancyId); // note: only an int due to legacy - will be a Guid

        ApplicationDetail GetApplication(Guid candidateId, int vacancyId);

        void ArchiveApplication(Guid candidateId, int vacancyId);

        void SaveApplication(Guid candidateId, int vacancyId, ApplicationDetail application);

        IList<ApplicationSummary> GetApplications(Guid candidateId);

        void SubmitApplication(Guid candidateId, int vacancyId);

        void UnlockAccount(string username, string accountUnlockCode);

        void ResetForgottenPassword(string username, string passwordCode, string newPassword);

        void DeleteApplication(Guid candidateId, int vacancyId);
    }
}
