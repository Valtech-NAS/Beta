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
            this.AddSaveRules();
            this.AddMandatoryRules();
        }
    }

    public class EducationViewModelServerValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelServerValidator()
        {
            this.AddSaveRules();
            this.AddMandatoryRules();
            this.AddServerRules();
        }
    }

    public class EducationViewModelSaveValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelSaveValidator()
        {
            this.AddSaveRules();
        }
    }

    internal static class EducationValidaitonRules
    {
        internal static void AddMandatoryRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .NotEmpty()
                .WithMessage(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.RequiredErrorText);

            validator.RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(EducationViewModelMessages.FromYearMessages.RequiredErrorText);

            validator.RuleFor(x => x.ToYear)
                .NotEmpty()
                .WithMessage(EducationViewModelMessages.ToYearMessages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.ToYear)
                .Must(BeBeforeOrEqual)
                .WithMessage(EducationViewModelMessages.ToYearMessages.BeforeOrEqualErrorText);

            validator.RuleFor(x => x.FromYear)
                .Must(BeNowOrInThePast)
                .WithMessage(EducationViewModelMessages.FromYearMessages.NotInFutureErrorText);
        }

        internal static void AddSaveRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .Length(0, 120)
                .WithMessage(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.TooLongErrorText)
                .Matches(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListRegularExpression)
                .WithMessage(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListErrorText);

            validator.RuleFor(x => x.FromYear)
                .Matches(EducationViewModelMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationViewModelMessages.FromYearMessages.WhiteListErrorText);

            validator.RuleFor(x => x.ToYear)
                .Matches(EducationViewModelMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationViewModelMessages.FromYearMessages.WhiteListErrorText);
        }

        private static bool BeBeforeOrEqual(EducationViewModel instance, string toYear)
        {
            if (string.IsNullOrEmpty(toYear))
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
            if (string.IsNullOrEmpty(fromYear))
            {
                //Will be picked up by required validator
                return true;
            }
            var from = int.Parse(fromYear);
            return from <= DateTime.Now.Year;
        }
    }
}