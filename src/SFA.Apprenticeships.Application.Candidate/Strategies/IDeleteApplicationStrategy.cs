namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IDeleteApplicationStrategy
    {
        void DeleteApplication(Guid applicationId);
    }
}