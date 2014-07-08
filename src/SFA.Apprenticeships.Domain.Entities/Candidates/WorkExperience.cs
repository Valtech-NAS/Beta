namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class WorkExperience
    {
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
    }
}
