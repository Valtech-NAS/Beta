
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
           // this.AddClientRules();
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
            modelState.Clear();
            
            var result = this.Validate(model);
            result.AddToModelState(modelState, string.Empty);

            if (!modelState.IsValid)
            {
                modelState.AddModelError(string.Empty, "Sorry, we didn't find a match for the location. Please try again.");
            }

            return modelState.IsValid;
        }
    }

    public static class VacancySearchValidatorRules
    {
        public static void AddBrowserRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
        }

        public static void AddCommonRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            validator.RuleFor(x => x.Location).NotEmpty();
            validator.RuleFor(x => x.Longitude).NotEmpty().OverridePropertyName("Location");
            //validator.RuleFor(x => x.Latitude).NotEmpty().OverridePropertyName("Location");
        }

        public static void AddServerRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
        }
    }
}