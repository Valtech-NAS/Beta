namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using StructureMap;

    public class CandidateBuilder
    {
        public CandidateBuilder(string emailAddress)
        {
            RegistrationBuilder = new RegistrationBuilder(emailAddress);

            Candidate = new Candidate
            {
                EntityId = UserBuilder.UserAndCandidateId,
                DateCreated = DateTime.Now
            };
        }

        public Candidate Candidate { get; private set; }

        public RegistrationBuilder RegistrationBuilder { get; private set; }

        public Candidate Build()
        {
            var repo = ObjectFactory.GetInstance<ICandidateWriteRepository>();
            var repoRead = ObjectFactory.GetInstance<ICandidateReadRepository>();

            Candidate.RegistrationDetails = RegistrationBuilder.Build();

            var candidateInRepo = repoRead.Get(Candidate.RegistrationDetails.EmailAddress);
            if (candidateInRepo == null)
            {
                repo.Save(Candidate);
            }
            else
            {
                Candidate.EntityId = candidateInRepo.EntityId;
            }

            return Candidate;
        }
    }
}
