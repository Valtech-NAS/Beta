namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;
    using Locations;

    public class RegistrationDetails
    {
        private string _emailAddress;

        public RegistrationDetails()
        {
            Address = new Address();
        }

        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public string EmailAddress
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_emailAddress))
                {
                    return _emailAddress.ToLower();
                }

                return _emailAddress;
            }
            set { _emailAddress = value; }
        }

        public string PhoneNumber { get; set; }

        public string AcceptedTermsAndConditionsVersion { get; set; }
    }
}
