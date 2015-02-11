namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using System.Linq;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.VacancySearch;

    public class TraineeshipSearchViewModelClientValidator : AbstractValidator<TraineeshipSearchViewModel>
    {
        public TraineeshipSearchViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class TraineeshipSearchViewModelServerValidator : AbstractValidator<TraineeshipSearchViewModel>
    {
        public TraineeshipSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public class TraineeshipSearchViewModelLocationValidator : AbstractValidator<TraineeshipSearchViewModel>
    {
        public TraineeshipSearchViewModelLocationValidator()
        {
            this.AddLocationRules();
        }
    }

    public static class TraineeshipSearchValidatorRules
    {
        public static void AddCommonRules(this AbstractValidator<TraineeshipSearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage(TraineeshipSearchViewModelMessages.LocationMessages.RequiredErrorText)
                .Length(2, 99)
                .WithMessage(TraineeshipSearchViewModelMessages.LocationMessages.LengthErrorText)
                .Matches(TraineeshipSearchViewModelMessages.LocationMessages.WhiteList)
                .WithMessage(TraineeshipSearchViewModelMessages.LocationMessages.WhiteListErrorText);
        }

        public static void AddServerRules(this AbstractValidator<TraineeshipSearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .Length(3, 99)
                .When(x => x.Location != null && !x.Location.Any(Char.IsDigit))
                .WithMessage(TraineeshipSearchViewModelMessages.LocationMessages.LengthErrorText);
        }

        public static void AddLocationRules(this AbstractValidator<TraineeshipSearchViewModel> validator)
        {
            // NOTE: no message here, 'no results' help text provides suggestions to user.
            validator.RuleFor(x => x.Location)
                .Must(HaveLatAndLongPopulated);
        }

        private static bool HaveLatAndLongPopulated(TraineeshipSearchViewModel instance, string location)
        {
            return instance.Latitude.HasValue && instance.Longitude.HasValue;
        }
    }
}