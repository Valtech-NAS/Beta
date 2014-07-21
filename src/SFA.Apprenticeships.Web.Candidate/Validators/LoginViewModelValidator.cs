namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Login;

    public class LoginViewModelClientValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class LoginViewModelServerValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class LoginValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<LoginViewModel> validator)
        {
            validator.RuleFor(model => model.EmailAddress)
                .NotEmpty()
                .WithMessage(LoginViewModelMessages.EmailAddressMessages.RequiredErrorText);

            validator.RuleFor(model => model.Password)
                .NotEmpty()
                .WithMessage(LoginViewModelMessages.PasswordMessages.RequiredErrorText);
        }
    }
}
