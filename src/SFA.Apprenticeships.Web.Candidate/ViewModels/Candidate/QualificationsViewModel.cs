namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Serializable]
    public class QualificationsViewModel
    {
        public string Grade  { get; set; }
        public bool IsPredicted { get; set; }
        public string QualificationType { get; set; }
        public string Subject { get; set; }
       // [UIHint("Year")]
        public int Year { get; set; }
    }
}