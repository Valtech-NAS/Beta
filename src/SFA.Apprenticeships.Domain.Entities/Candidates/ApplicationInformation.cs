namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using System.Collections.Generic;

    public class ApplicationInformation
    {
        public ApplicationInformation()
        {
            EducationHistory = new List<Education>();
            Qualifications = new List<Qualification>();
            WorkExperience = new List<WorkExperience>();
            AboutYou = new AboutYou();
        }

        public IList<Education> EducationHistory { get; set; } //note: for legacy integration just need the latest
        public IList<Qualification> Qualifications { get; set; }
        public IList<WorkExperience> WorkExperience { get; set; }
        public AboutYou AboutYou { get; set; }
    }
}
