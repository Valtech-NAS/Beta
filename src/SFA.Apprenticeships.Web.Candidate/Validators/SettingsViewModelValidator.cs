namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Account;

    public class SettingsViewModelClientValidator : AbstractValidator<SettingsViewModel>
    {
        public SettingsViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class SettingsViewModelServerValidator : AbstractValidator<SettingsViewModel>
    {
        public SettingsViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public static class SettingsViewModelValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<SettingsViewModel> validator)
        {
            validator.RuleFor(x => x.DateOfBirth).SetValidator(new DateOfBirthViewModelClientValidator());
            validator.RuleFor(x => x.Address).SetValidator(new AddressViewModelValidator());

            validator.RuleFor(x => x.Firstname)
                .Length(0, 35)
                .WithMessage(SettingsViewModelMessages.FirstnameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(SettingsViewModelMessages.FirstnameMessages.RequiredErrorText)
                .Matches(SettingsViewModelMessages.FirstnameMessages.WhiteListRegularExpression)
                .WithMessage(SettingsViewModelMessages.FirstnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Lastname)
                .Length(0, 35)
                .WithMessage(SettingsViewModelMessages.LastnameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(SettingsViewModelMessages.LastnameMessages.RequiredErrorText)
                .Matches(SettingsViewModelMessages.LastnameMessages.WhiteListRegularExpression)
                .WithMessage(SettingsViewModelMessages.LastnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.PhoneNumber)
                .Length(8, 16)
                .WithMessage(SettingsViewModelMessages.PhoneNumberMessages.LengthErrorText)
                .NotEmpty()
                .WithMessage(SettingsViewModelMessages.PhoneNumberMessages.RequiredErrorText)
                .Matches(SettingsViewModelMessages.PhoneNumberMessages.WhiteListRegularExpression)
                .WithMessage(SettingsViewModelMessages.PhoneNumberMessages.WhiteListErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<SettingsViewModel> validator)
        {
            validator.RuleFor(x => x.DateOfBirth).SetValidator(new DateOfBirthViewModelServerValidator());
        }
    }
}
