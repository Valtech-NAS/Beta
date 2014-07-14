namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Register;

    public class RegisterViewModelClientValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class RegisterViewModelServerValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class RegisterValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<RegisterViewModel> validator)
        {
            validator.RuleFor(x => x.DateOfBirth).SetValidator(new DateOfBirthViewModelClientValidator());
            validator.RuleFor(x => x.Address).SetValidator(new AddressViewModelValidator());

            validator.RuleFor(x => x.Firstname)
                .Length(0, 35)
                .WithMessage(RegisterViewModelMessages.FirstnameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.FirstnameMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.FirstnameMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.FirstnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Lastname)
                .Length(0, 35)
                .WithMessage(RegisterViewModelMessages.LastnameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.LastnameMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.LastnameMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.LastnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.EmailAddress)
                .Length(0, 100)
                .WithMessage(RegisterViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            validator.RuleFor(x => x.PhoneNumber)
                .Length(8, 16)
                .WithMessage(RegisterViewModelMessages.PhoneNumberMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.PhoneNumberMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.PhoneNumberMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.PhoneNumberMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Password)
                .Length(8, 127)
                .WithMessage(RegisterViewModelMessages.PasswordMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(RegisterViewModelMessages.PasswordMessages.RequiredErrorText)
                .Matches(RegisterViewModelMessages.PasswordMessages.WhiteListRegularExpression)
                .WithMessage(RegisterViewModelMessages.PasswordMessages.WhiteListErrorText);

            validator.RuleFor(x => x.HasAcceptedTermsAndConditions)
                .Equal(true)
                .WithMessage(RegisterViewModelMessages.TermsAndConditions.MustAcceptTermsAndConditions);
        }

        internal static void AddServerRules(this AbstractValidator<RegisterViewModel> validator)
        {
            validator.RuleFor(x => x.DateOfBirth).SetValidator(new DateOfBirthViewModelServerValidator());
        }
    }
}