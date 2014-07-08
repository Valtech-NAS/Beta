namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Candidates;

    public interface ICandidateReadRepository : IReadRepository<Candidate> { }

    public interface ICandidateWriteRepository : IWriteRepository<Candidate> { }
}
