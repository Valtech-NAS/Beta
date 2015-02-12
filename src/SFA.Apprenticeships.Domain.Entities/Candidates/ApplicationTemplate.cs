namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System.Collections.Generic;

    public class ApplicationTemplate
    {
        public ApplicationTemplate()
        {
            // note: might become a collection, for now allow to be null: EducationHistory = new Education();
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