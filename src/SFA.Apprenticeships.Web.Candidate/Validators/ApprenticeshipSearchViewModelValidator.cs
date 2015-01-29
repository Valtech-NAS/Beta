namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using System.Linq;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies;
    using FluentValidation;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchViewModelClientValidator : AbstractValidator<ApprenticeshipSearchViewModel>
    {
        public ApprenticeshipSearchViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class ApprenticeshipSearchViewModelServerValidator : AbstractValidator<ApprenticeshipSearchViewModel>
    {
        public ApprenticeshipSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
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
                .When(x => !VacancyHelper.IsVacancyReference(x.Keywords))
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.RequiredErrorText)
                .Length(2, 99)
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.LengthErrorText)
                .Matches(ApprenticeshipSearchViewModelMessages.LocationMessages.WhiteList)
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Keywords)
                .Matches(ApprenticeshipSearchViewModelMessages.KeywordMessages.WhiteList)
                .WithMessage(ApprenticeshipSearchViewModelMessages.KeywordMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Category)
                .NotEmpty()
                .When(x => x.SearchMode == ApprenticeshipSearchMode.Category)
                .WithMessage(ApprenticeshipSearchViewModelMessages.CategoryMessages.RequiredErrorText);
        }

        public static void AddServerRules(this AbstractValidator<ApprenticeshipSearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .Length(3, 99)
                .When(x => x.Location != null && !x.Location.Any(Char.IsDigit) && !VacancyHelper.IsVacancyReference(x.Keywords))
                .WithMessage(ApprenticeshipSearchViewModelMessages.LocationMessages.LengthErrorText);
        }

        public static void AddLocationRules(this AbstractValidator<ApprenticeshipSearchViewModel> validator)
        {
            // NOTE: no message here, 'no results' help text provides suggestions to user.
            validator.RuleFor(x => x.Location)
                .Must(HaveLatAndLongPopulated)
                .When(x => !VacancyHelper.IsVacancyReference(x.Keywords));
        }

        private static bool HaveLatAndLongPopulated(ApprenticeshipSearchViewModel instance, string location)
        {
            return instance.Latitude.HasValue && instance.Longitude.HasValue;
        }
    }
}