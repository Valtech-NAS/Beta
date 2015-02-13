namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Account;

    public class VerifyMobileViewModelServerValidator : AbstractValidator<VerifyMobileViewModel>
    {
        public VerifyMobileViewModelServerValidator()
        {
            this.AddCommonRules();
        }
    }

    public class VerifyMobileViewModelClientValidator : AbstractValidator<VerifyMobileViewModel>
    {
        public VerifyMobileViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class VerifyMobileValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<VerifyMobileViewModel> validator)
        {
            validator.RuleFor(model => model.VerifyMobileCode)
                .NotEmpty()
                .WithMessage(VerifyMobileViewModelMessages.VerifyMobileCodeMessages.RequiredErrorText)
                .Length(4, 4)
                .WithMessage(VerifyMobileViewModelMessages.VerifyMobileCodeMessages.LengthErrorText);
        }
    }
}
