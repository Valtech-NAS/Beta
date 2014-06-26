namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using Entities.Candidates;

    public interface ICandidateRepository : IWriteRepository<Candidate>, IReadRepository<Candidate> {}
}
