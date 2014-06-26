
namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System.Web.Mvc;
    using FluentValidation;
    using FluentValidation.Mvc;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class VacancySearchClientSideValidator : AbstractValidator<VacancySearchViewModel>
    {
        public VacancySearchClientSideValidator()
        {
            this.AddCommonRules();
            this.AddBrowserRules();
        }
    }

    public class VacancySearchValidator : AbstractValidator<VacancySearchViewModel>, IValidateModel<VacancySearchViewModel>
    {
        public VacancySearchValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }

        public bool Validate(VacancySearchViewModel model, ModelStateDictionary modelState)
        {                    
            var result = this.Validate(model);
            result.AddToModelState(modelState, string.Empty);

            if (!modelState.IsValid)
            {
                modelState.Clear();
                modelState.AddModelError("Location", "Sorry, we didn't find a match for the location entered");
            }

            return modelState.IsValid;
        }
    }

    public static class VacancySearchValidatorRules
    {
        public static void AddBrowserRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            //validator.RuleFor(x => x.Latitude)
            //    .NotEmpty()
            //    .WithMessage("Sorry, we didn't find a match for the location. Please try again.");
        }

        public static void AddCommonRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Please provide a location.")
                .Length(3, 99)
                .WithMessage("Location name or postcode must be 3 or more characters.");
        }

        public static void AddServerRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            validator.RuleFor(x => x.Latitude).Must(y => y.HasValue);
            validator.RuleFor(x => x.Longitude).Must(y => y.HasValue);
        }
    }
}