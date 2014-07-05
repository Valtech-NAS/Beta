namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;

    public class Qualification
    {
        public string QualificationType { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public bool IsPredicted { get; set; }
        public int Year { get; set; }
    }
}
