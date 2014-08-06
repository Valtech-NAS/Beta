namespace SFA.Apprenticeships.Web.Candidate.SpecBind.IntegrationTests.Builders
{
    using System;
    using Domain.Entities.Users;

    public class RegistrationBuilder
    {
        public RegistrationBuilder(string emailAddress)
        {
            RegistrationDetails = new RegistrationDetails
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
            };
        }

        public RegistrationDetails RegistrationDetails { get; private set; }

        public RegistrationDetails Build()
        {
            return RegistrationDetails;
        }
    }
}