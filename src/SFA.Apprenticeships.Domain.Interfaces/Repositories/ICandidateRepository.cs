namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Candidates;

    public interface ICandidateReadRepository : IReadRepository<Candidate>
    {
        Candidate Get(Guid id, bool errorIfNotFound);
        Candidate Get(string username, bool errorIfNotFound = true);
    }

    public interface ICandidateWriteRepository : IWriteRepository<Candidate>
    {
    }
}