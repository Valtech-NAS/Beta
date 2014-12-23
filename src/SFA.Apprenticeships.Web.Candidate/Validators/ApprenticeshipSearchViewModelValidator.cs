namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchViewModelClientValidator : AbstractValidator<ApprenticeshipSearchViewModel>
    {
        public ApprenticeshipSearchViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class ApprenticeshipSearchViewModelLocationValidator : AbstractValidator<ApprenticeshipSearchViewModel>
    {
        public ApprenticeshipSearchViewModelLocationValidator()
        {
            this.AddLocationRules();
        }
    }

    public static class ApprenticeshipSearchValidatorRules
    {
        public static void AddCommonRules(this AbstractValidator<ApprenticeshipSearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.RequiredErrorText)
                .Length(2, 99)
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.LengthErrorText)
                .Matches(ApprenticeshipSearchViewModelMessages.LocationMessages.WhiteList)
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Keywords)
                .Matches(ApprenticeshipSearchViewModelMessages.KeywordMessages.WhiteList)
                .WithMessage(ApprenticeshipSearchViewModelMessages.KeywordMessages.WhiteListErrorText);
        }

        public static void AddLocationRules(this AbstractValidator<ApprenticeshipSearchViewModel> validator)
        {
            // NOTE: no message here, 'no results' help text provides suggestions to user.
            validator.RuleFor(x => x.Location)
                .Must(HaveLatAndLongPopulated);
        }

        private static bool HaveLatAndLongPopulated(ApprenticeshipSearchViewModel instance, string location)
        {
            return instance.Latitude.HasValue && instance.Longitude.HasValue;
        }
    }
}