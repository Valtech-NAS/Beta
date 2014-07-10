namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using System.Collections.Generic;

    public class ApplicationTemplate
    {
        public ApplicationTemplate()
        {
            EducationHistory = new Education();
            Qualifications = new List<Qualification>();
            WorkExperience = new List<WorkExperience>();
            AboutYou = new AboutYou();
        }

        public Education EducationHistory { get; set; }
        public IList<Qualification> Qualifications { get; set; }
        public IList<WorkExperience> WorkExperience { get; set; }
        public AboutYou AboutYou { get; set; }
    }
}
