namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Login;
    using ViewModels.Register;

    public class ForgottenPasswordViewModelClientValidator : AbstractValidator<ForgottenPasswordViewModel>
    {
        public ForgottenPasswordViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class ForgottenPasswordViewModelServerValidator : AbstractValidator<ForgottenPasswordViewModel>
    {
        public ForgottenPasswordViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public static class ForgottenPasswordViewModelValidationRules
    {
        public static void AddCommonRules(this AbstractValidator<ForgottenPasswordViewModel> validator)
        {
            validator.RuleFor(x => x.EmailAddress)
                .Length(0, 100)
                .WithMessage(ForgottenPasswordViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(ForgottenPasswordViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(ForgottenPasswordViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(ForgottenPasswordViewModelMessages.EmailAddressMessages.WhiteListErrorText);
        }
    }
}