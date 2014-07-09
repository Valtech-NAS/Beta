namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class EducationViewModelClientValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    public class EducationViewModelServerValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class EducationValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .Length(0, 120)
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.RequiredErrorText)
                .Matches(EducationMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListErrorText);

            validator.RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(EducationMessages.FromYearMessages.RequiredErrorText)
                .Matches(EducationMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.FromYearMessages.WhiteListErrorText);

            validator.RuleFor(x => x.ToYear)
                .NotEmpty()
                .WithMessage(EducationMessages.ToYearMessages.RequiredErrorText)
                .Matches(EducationMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.FromYearMessages.WhiteListErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.ToYear)
                .Must(BeBeforeOrEqual)
                .WithMessage(EducationMessages.ToYearMessages.BeforeOrEqualErrorText);

            validator.RuleFor(x => x.FromYear)
                .Must(BeNowOrInThePast)
                .WithMessage(EducationMessages.FromYearMessages.NotInFutureErrorText);
                
        }

        private static bool BeBeforeOrEqual(EducationViewModel instance, string toYear)
        {
            if (toYear == null)
            {
                //Will be picked up by required validator
                return true;
            }
            var to = int.Parse(toYear);
            var from = int.Parse(instance.FromYear);
            return from <= to;
        }

        private static bool BeNowOrInThePast(EducationViewModel instance, string fromYear)
        {
            if (fromYear == null)
            {
                //Will be picked up by required validator
                return true;
            }
            var from = int.Parse(fromYear);
            return from <= DateTime.Now.Year;
        }
    }
}