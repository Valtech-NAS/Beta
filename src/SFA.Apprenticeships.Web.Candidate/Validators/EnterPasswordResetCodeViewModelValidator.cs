namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Register;

    public class EnterPasswordResetCodeViewModelClientValidator : AbstractValidator<EnterPasswordResetCodeViewModel>
    {
        public EnterPasswordResetCodeViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class EnterPasswordResetCodeViewModelServerValidator : AbstractValidator<EnterPasswordResetCodeViewModel>
    {
        public EnterPasswordResetCodeViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public static class EnterPasswordResetCodeViewModelValidatorRules
    {
        public static void AddCommonRules(this AbstractValidator<EnterPasswordResetCodeViewModel> validator)
        {
            validator.RuleFor(x => x.PasswordResetCode)
                .NotEmpty()
                .WithMessage(EnterPasswordResetCodeViewModelMessages.PasswordResetCode.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(EnterPasswordResetCodeViewModelMessages.PasswordResetCode.LengthErrorText);
        }

        public static void AddServerRules(this AbstractValidator<EnterPasswordResetCodeViewModel> validator)
        {
            validator.RuleFor(x => x.PasswordResetCode)
                .Must(IncorrectPasswordResetCode)
                .WithMessage(EnterPasswordResetCodeViewModelMessages.PasswordResetCode.WrongPasswordResetCodeErrorText);
        }

        private static bool IncorrectPasswordResetCode(EnterPasswordResetCodeViewModel model, string passwordResetCode)
        {
            return passwordResetCode != null &&
                   (!string.IsNullOrEmpty(passwordResetCode) && model.IsPasswordResetCodeCorrect);
        }
    }
}