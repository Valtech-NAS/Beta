namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using System.Collections.Generic;
    using Locations;
    using Users;

    public class Candidate : User
    {
        public Candidate()
        {
            Roles = UserRoles.Candidate;
            EducationHistory = new List<Education>();
            Qualifications = new List<Qualification>();
            WorkExperience = new List<WorkExperience>();
        }

        //todo: add Candidate.Title?
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; } //todo: Candidate.Mobile and/or Landline?

        public IEnumerable<Education> EducationHistory { get; set; }
        public IEnumerable<Qualification> Qualifications { get; set; }
        public IEnumerable<WorkExperience> WorkExperience { get; set; }

        //todo: confirm whether these only belong on application or within this profile too
        public string AboutYouStrengths { get; set; }
        public string AboutYouImprovements { get; set; }
        public string AboutYouHobbiesAndInterests { get; set; }
        public string AboutYouSupport { get; set; }
    }
}
