namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Locations;
    using Validators;

    [Validator(typeof(RegisterViewModelClientValidator))]
    public class RegisterViewModel : ViewModelBase
    {
        [Display(Name = RegisterViewModelMessages.FirstnameMessages.LabelText)]
        public string Firstname { get; set; }

        [Display(Name = RegisterViewModelMessages.LastnameMessages.LabelText)]
        public string Lastname { get; set; }

        public DateViewModel DateOfBirth { get; set; }

        public AddressViewModel Address{ get; set; }

        [Display(Name = RegisterViewModelMessages.EmailAddressMessages.LabelText, Description = RegisterViewModelMessages.EmailAddressMessages.HintText)]
        public string EmailAddress { get; set; }

        [Display(Name = RegisterViewModelMessages.PhoneNumberMessages.LabelText, Description = RegisterViewModelMessages.PhoneNumberMessages.HintText)]
        public string PhoneNumber { get; set; }

        [Display(Name = RegisterViewModelMessages.PasswordMessages.LabelText)]
        public string Password { get; set; }

        [Display(Name = RegisterViewModelMessages.ConfirmPasswordMessages.LabelText)]
        public string ConfirmPassword { get; set; }

        [Display(Name = RegisterViewModelMessages.TermsAndConditions.LabelText)]
        public bool HasAcceptedTermsAndConditions { get; set; }

        public bool IsUsernameAvailable { get; set; }
    }
}