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
        [Display(Name = EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.LabelText)]
        public string NameOfMostRecentSchoolCollege { get; set; }

        [Display(Name = EducationViewModelMessages.FromYearMessages.LabelText)]
        public string FromYear { get; set; }

        [Display(Name = EducationViewModelMessages.ToYearMessages.LabelText)]
        public string ToYear { get; set; }
    }
}