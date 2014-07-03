namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System.Web.Mvc;
    using Constants.ViewModels;
    using FluentValidation;
    using FluentValidation.Mvc;
    using ViewModels.VacancySearch;

    public class VacancySearchClientSideValidator : AbstractValidator<VacancySearchViewModel>
    {
        public VacancySearchClientSideValidator()
        {
            this.AddCommonRules();
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
            var result = Validate(model);
            result.AddToModelState(modelState, string.Empty);

            if (!modelState.IsValid)
            {
                modelState.Clear();
                modelState.AddModelError("Location", VacancySearchMessages.LocationMessages.NoResultsErrorText);
            }

            return modelState.IsValid;
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

        public static void AddServerRules(this AbstractValidator<VacancySearchViewModel> validator)
        {
            validator.RuleFor(x => x.Latitude).Must(y => y.HasValue);
            validator.RuleFor(x => x.Longitude).Must(y => y.HasValue);
        }
    }
}