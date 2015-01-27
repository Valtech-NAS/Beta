namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using System.Configuration;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Configuration;
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
            var configuration = ObjectFactory.GetInstance<IConfigurationManager>();
#pragma warning restore 0618

            Candidate.RegistrationDetails = RegistrationBuilder.Build();
            Candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = configuration.GetAppSetting<string>("TermsAndConditionsVersion");
            Candidate.CommunicationPreferences = new CommunicationPreferences();

            var candidateInRepo = repoRead.Get(Candidate.RegistrationDetails.EmailAddress);

            if (candidateInRepo != null)
            {
                Candidate.EntityId = candidateInRepo.EntityId;
            }

            repo.Save(Candidate);

            return Candidate;
        }
    }
}
