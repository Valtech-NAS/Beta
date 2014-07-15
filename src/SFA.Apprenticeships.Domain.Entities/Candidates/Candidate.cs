namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class Candidate : BaseEntity
    {
        public Candidate()
        {
            RegistrationDetails = new RegistrationDetails();
            ApplicationTemplate = new ApplicationTemplate();
        }

        public int LegacyCandidateId { get; set; } // temporary "weak link" to legacy candidate record

        public RegistrationDetails RegistrationDetails { get; set; }

        public ApplicationTemplate ApplicationTemplate { get; set; }

        //public VacancyProfile VacancyProfile { get; set; } //todo: add candidate's vacancy profile (matching)
    }
}
