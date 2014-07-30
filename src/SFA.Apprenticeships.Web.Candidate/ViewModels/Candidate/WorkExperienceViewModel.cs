namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;

    [Serializable]
    public class WorkExperienceViewModel
    {
        public string Description { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
    }
}