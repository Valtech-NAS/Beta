namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Users;

    public class Candidate : User
    {
        public Candidate()
        {
            Roles = UserRoles.Candidate;
            Status = UserStatuses.Unknown;
            PersonalDetails = new PersonalDetails();
            ApplicationTemplate = new ApplicationInformation();
        }

        public int LegacyCandidateId { get; set; } // temporary "weak link" to legacy candidate record

        public PersonalDetails PersonalDetails { get; set; }

        public ApplicationInformation ApplicationTemplate { get; set; }

        //public VacancyProfile VacancyProfile { get; set; } //todo: add candidate's vacancy profile
    }
}
