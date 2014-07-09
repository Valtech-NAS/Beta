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
            RegistrationDetails = new RegistrationDetails();
            ApplicationTemplate = new ApplicationTemplate();
        }

        public int LegacyCandidateId { get; set; } // temporary "weak link" to legacy candidate record

        public RegistrationDetails RegistrationDetails { get; set; }

        public ApplicationTemplate ApplicationTemplate { get; set; }

        //public VacancyProfile VacancyProfile { get; set; } //todo: add candidate's vacancy profile (matching)
    }
}
