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

            Candidate.RegistrationDetails = RegistrationBuilder.Build();

            repo.Save(Candidate);

            return Candidate;
        }
    }
}
