namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof (QualificationViewModelValidator))]
    public class QualificationsViewModel
    {
        public string Grade { get; set; }
        public bool IsPredicted { get; set; }
        public string QualificationType { get; set; }
        public string Subject { get; set; }
        public string Year { get; set; }
    }
}