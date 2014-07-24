namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof (PasswordResetViewModelClientValidator))]
    public class PasswordResetViewModel
    {
        public string EmailAddress { get; set; }

        [Display(Name = PasswordResetViewModelMessages.PasswordMessages.LabelText,
            Description = PasswordResetViewModelMessages.PasswordMessages.HintText)]
        public string Password { get; set; }

        public bool IsPasswordResetSuccessful { get; set; }
    }
}