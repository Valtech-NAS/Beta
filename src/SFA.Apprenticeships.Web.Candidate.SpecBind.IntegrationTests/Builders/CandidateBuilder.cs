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
            Candidate = new Candidate
            {
                EntityId = new Guid(candidateId),
                DateCreated = DateTime.Now,
                RegistrationDetails =
                {
                    EmailAddress = emailAddress,
                    FirstName = "John",
                    MiddleNames = string.Empty,
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1970, 12, 31),
                    Address =
                    {
                        AddressLine1 = "103 Crawley Drive",
                        AddressLine2 = "The Village Green",
                        AddressLine3 = "Hemel Hempstead",
                        AddressLine4 = "Hertfordhsire",
                        Postcode = "HP2 6AL"
                    },
                    PhoneNumber = "01221234567"
                }
            };
        }

        public Candidate Candidate { get; private set; }

        public Candidate Build()
        {
            var repo = ObjectFactory.GetInstance<ICandidateWriteRepository>();

            repo.Save(Candidate);

            return Candidate;
        }
    }
}
