namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests.Helpers
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;

    public static class TestCandidateHelper
    {
        public static Candidate CreateFakeCandidate()
        {
            var candidate = new Candidate
            {
                EntityId = Guid.NewGuid(),
                LegacyCandidateId = 201,
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = string.Format("john.doe+{0}@example.com", DateTime.Now.Ticks) ,
                    DateOfBirth = new DateTime(1980, 1, 1),
                    PhoneNumber = "01683200911",
                    Address = new Address
                    {
                        AddressLine1 = "10 Acacia Avenue",
                        AddressLine3 = "Some House",
                        AddressLine4 = "Some Town",
                        Postcode = "FF2 7AL",
                        AddressLine2 = "East Nether"
                    },
                }
            };

            return candidate;
        }
    }
}
