namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface ISaveApprenticeshipApplicationStrategy
    {
        ApprenticeshipApplicationDetail SaveApplication(Guid candidateId, int vacancyId, ApprenticeshipApplicationDetail apprenticeshipApplication);
    }
}