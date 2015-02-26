namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Login;

    public class AccountUnlockViewModelClientValidator : AbstractValidator<AccountUnlockViewModel>
    {
        public AccountUnlockViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class AccountUnlockViewModelServerValidator : AbstractValidator<AccountUnlockViewModel>
    {
        public AccountUnlockViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public class ResendAccountUnlockCodeViewModelServerValidator : AbstractValidator<AccountUnlockViewModel>
    {
        public ResendAccountUnlockCodeViewModelServerValidator()
        {
            this.AddEmailRules();
        }
    }

    public static class AccountUnlockValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<AccountUnlockViewModel> validator)
        {
            validator.RuleFor(model => model.AccountUnlockCode)
                .NotEmpty()
                .WithMessage(AccountUnlockViewModelMessages.AccountUnlockCodeMessages.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(AccountUnlockViewModelMessages.AccountUnlockCodeMessages.LengthErrorText)
                .Matches(AccountUnlockViewModelMessages.AccountUnlockCodeMessages.WhiteListRegularExpression)
                .WithMessage(AccountUnlockViewModelMessages.AccountUnlockCodeMessages.WhiteListErrorText);

            AddEmailRules(validator);
        }

        internal static void AddEmailRules(this AbstractValidator<AccountUnlockViewModel> validator)
        {
            validator.RuleFor(model => model.EmailAddress)
                .NotEmpty()
                .WithMessage(ActivationCodeMessages.EmailAddressMessages.RequiredErrorText);
        }
    }
}
