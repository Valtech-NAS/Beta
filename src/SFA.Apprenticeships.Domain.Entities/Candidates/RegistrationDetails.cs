namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Locations;

    public class RegistrationDetails
    {
        public RegistrationDetails()
        {
            Address = new Address();
        }

        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null)
                {
                    return null;
                }

                return FirstName + " " + LastName;
            }
        }
    }
}
