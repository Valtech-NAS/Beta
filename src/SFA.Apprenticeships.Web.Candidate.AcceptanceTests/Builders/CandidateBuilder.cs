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
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var repo = ObjectFactory.GetInstance<ICandidateWriteRepository>();
            var repoRead = ObjectFactory.GetInstance<ICandidateReadRepository>();
#pragma warning restore 0618

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
