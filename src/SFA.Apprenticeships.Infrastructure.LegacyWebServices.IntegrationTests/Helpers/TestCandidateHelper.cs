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
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "NAS",
                    LastName = "Exemplar",
                    EmailAddress = string.Format("nas.exemplar+{0}@gmail.com", DateTime.Now.Ticks) ,
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
