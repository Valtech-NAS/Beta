namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof (ForgottenPasswordViewModelClientValidator))]
    public class ForgottenPasswordViewModel
    {
        [Display(Name = ForgottenPasswordViewModelMessages.EmailAddressMessages.LabelText,
            Description = ForgottenPasswordViewModelMessages.EmailAddressMessages.HintText)]
        public string EmailAddress { get; set; }
    }
}