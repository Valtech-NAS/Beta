namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;

    public interface IArchiveApplicationStrategy
    {
        void ArchiveApplication(Guid applicationId);
    }
}
