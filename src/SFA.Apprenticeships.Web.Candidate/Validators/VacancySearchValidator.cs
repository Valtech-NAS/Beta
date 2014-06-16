
namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class VacancySearchValidator : AbstractValidator<VacancySearchViewModel>
    {
        public VacancySearchValidator()
        {
            this.AddBrowserRules();
            this.AddCommonRules();
        }
    }

    public class VacancySearchFullValidator : AbstractValidator<VacancySearchViewModel>
    {
        public VacancySearchFullValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public static class VacancySearchValidatorValidatorRules
    {
        public static void AddBrowserRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            //todo: add validation rules
        }

        public static void AddCommonRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            //todo: add validation rules
            validator.RuleFor(x => x.Location).NotEmpty();

            validator.RuleFor(x => x.Longitude).NotEmpty().OverridePropertyName("Location");
            validator.RuleFor(x => x.Latitude).NotEmpty().OverridePropertyName("Location");
        }

        public static void AddServerRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            //todo: add validation rules
        }
    }
}