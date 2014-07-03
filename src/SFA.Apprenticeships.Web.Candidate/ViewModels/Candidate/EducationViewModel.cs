namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof(EducationViewModelClientValidator))]
    public class EducationViewModel
    {
        [Display(Name = EducationMessages.NameOfMostRecentSchoolCollegeMessages.LabelText)]
        public string NameOfMostRecentSchoolCollege { get; set; }

        [Display(Name = EducationMessages.FromYearMessages.LabelText)]
        public string FromYear { get; set; }

        [Display(Name = EducationMessages.ToYearMessages.LabelText)]
        public string ToYear { get; set; }
    }
}