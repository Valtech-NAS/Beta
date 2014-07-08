namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.VacancySearch;

    public class VacancySearchViewModelClientValidator : AbstractValidator<VacancySearchViewModel>
    {
        public VacancySearchViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class VacancySearchViewModelLocationValidator : AbstractValidator<VacancySearchViewModel>
    {
        public VacancySearchViewModelLocationValidator()
        {
            this.AddLocationRules();
        }
    }

    public static class VacancySearchValidatorRules
    {
        public static void AddCommonRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage(VacancySearchMessages.LocationMessages.RequiredErrorText)
                .Length(3, 99)
                .WithMessage(VacancySearchMessages.LocationMessages.LengthErrorText)
                .Matches(VacancySearchMessages.LocationMessages.WhiteList)
                .WithMessage(VacancySearchMessages.LocationMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Keywords)
                .Matches(VacancySearchMessages.KeywordMessages.WhiteList)
                .WithMessage(VacancySearchMessages.KeywordMessages.WhiteListErrorText);
        }

        public static void AddLocationRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .Must(HaveLatAndLongPopulated)
                .WithMessage(VacancySearchMessages.LocationMessages.NoResultsErrorText);
        }

        private static bool HaveLatAndLongPopulated(VacancySearchViewModel instance, string location)
        {
            return instance.Latitude.HasValue && instance.Longitude.HasValue;
        }
    }
}