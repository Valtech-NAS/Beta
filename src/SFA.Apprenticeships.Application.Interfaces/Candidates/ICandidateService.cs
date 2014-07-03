namespace SFA.Apprenticeships.Application.Interfaces.Candidates
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;

    /// <summary>
    /// For candidate users to manage their profile and other dashboard entities
    /// </summary>
    public interface ICandidateService
    {
        Candidate GetCandidate(int candidateId);

        Candidate SaveCandidate(Candidate candidate);

        ApplicationDetail SaveApplication(ApplicationDetail application);

        void SubmitApplication(ApplicationDetail application);
    }
}
