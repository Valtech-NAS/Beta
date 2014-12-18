namespace SFA.Apprenticeships.Application.Candidate.Strategies.Apprenticeships
{
    using System;
    using Domain.Entities.Applications;

    public interface ICreateApprenticeshipApplicationStrategy
    {
        ApprenticeshipApplicationDetail CreateApplication(Guid candidateId, int vacancyId);
    }
}
