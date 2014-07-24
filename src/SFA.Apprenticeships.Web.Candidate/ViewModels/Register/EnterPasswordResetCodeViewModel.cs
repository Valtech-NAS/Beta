namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof (EnterPasswordResetCodeViewModelClientValidator))]
    public class EnterPasswordResetCodeViewModel
    {
        public string EmailAddress { get; set; }

        [Display(Name = EnterPasswordResetCodeViewModelMessages.PasswordResetCode.LabelText,
            Description = PasswordResetViewModelMessages.PasswordMessages.HintText)]
        public string PasswordResetCode { get; set; }

        public bool IsPasswordResetCodeCorrect { get; set; }
    }
}