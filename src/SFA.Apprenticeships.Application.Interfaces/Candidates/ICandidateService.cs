namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    /// <summary>
    /// For candidate users to register, manage their profile and other dashboard entities
    /// </summary>
    public interface ICandidateService
    {
        bool IsUsernameAvailable(string username);

        Candidate RegisterCandidate(Candidate newCandidate, string password);

        Candidate Authenticate(string username, string password);

        Candidate GetCandidate(Guid id);

        Candidate SaveCandidate(Candidate candidate);

        ApplicationDetail SaveApplication(ApplicationDetail application);

        void SubmitApplication(ApplicationDetail application);
    }
}
