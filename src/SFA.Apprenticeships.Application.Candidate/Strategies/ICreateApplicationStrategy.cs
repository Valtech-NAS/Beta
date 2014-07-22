namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public interface ICreateApplicationStrategy
    {
        ApplicationDetail CreateApplication(Guid candidateId, int vacancyId);
    }
}
