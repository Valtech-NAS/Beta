namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Builders
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using StructureMap;

    public class CandidateBuilder
    {
        public CandidateBuilder(string candidateId, string emailAddress)
        {
            RegistrationBuilder = new RegistrationBuilder(emailAddress);

            Candidate = new Candidate
            {
                EntityId = new Guid(candidateId),
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
