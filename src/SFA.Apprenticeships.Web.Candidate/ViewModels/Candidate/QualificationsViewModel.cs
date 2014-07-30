namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;

    [Serializable]
    public class QualificationsViewModel
    {
        public string Grade  { get; set; }
        public bool IsPredicted { get; set; }
        public string QualificationType { get; set; }
        public string Subject { get; set; }
        public int Year { get; set; }
    }
}