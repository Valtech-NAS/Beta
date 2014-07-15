using System;
using FluentValidation;
using FluentValidation.Results;
using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;

namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    public class ActivationViewModelClientValidator : AbstractValidator<ActivationViewModel>
    {
        public ActivationViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class ActivationViewModelServerValidator : AbstractValidator<ActivationViewModel>
    {
        public ActivationViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public static class ActivationViewModelValidationRules
    {
        public static void AddCommonRules(this AbstractValidator<ActivationViewModel> validator)
        {
            validator.RuleFor(x => x.ActivationCode)
                .NotEmpty()
                .WithMessage(ActivationCodeMessages.ActivationCode.RequiredErrorText)
                .Length(6, 6)
                .WithMessage(ActivationCodeMessages.ActivationCode.LengthErrorText);
        }

        public static void AddServerRules(this AbstractValidator<ActivationViewModel> validator)
        {
            validator.RuleFor(x => x.ActivationCode)
                .Must(BeTheSameAsCodeHeldOnRecord)
                .WithMessage(ActivationCodeMessages.ActivationCode.WrongActivationCodeErrorText);
        }

        private static bool BeTheSameAsCodeHeldOnRecord(ActivationViewModel model, string activationCode)
        {
            return activationCode != null && (!string.IsNullOrEmpty(activationCode) && model.IsActivated);
        }
    }
}