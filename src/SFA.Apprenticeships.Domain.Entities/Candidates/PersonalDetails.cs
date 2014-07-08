namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Locations;

    public class PersonalDetails
    {
        public PersonalDetails()
        {
            Address = new Address();
        }

        //todo: add Candidate.Title?
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; } //todo: Candidate.Mobile and/or Landline? PhoneType??
    }
}
