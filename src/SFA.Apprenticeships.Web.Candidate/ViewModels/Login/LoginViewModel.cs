namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Login
{
    using System.ComponentModel.DataAnnotations;
    using FluentValidation.Attributes;
    using Constants.ViewModels;
    using Validators;

    [Validator(typeof(LoginViewModelClientValidator))]
    public class LoginViewModel
    {
        [Display(Name = LoginViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = LoginViewModelMessages.PasswordMessages.LabelText)]
        public string Password { get; set; }
    }
}
