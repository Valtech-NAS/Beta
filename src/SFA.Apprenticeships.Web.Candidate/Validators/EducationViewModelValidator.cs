namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using Helpers;
    using ViewModels.Candidate;

    public class EducationViewModelClientValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelClientValidator()
        {
            this.AddMandatoryRules();
            this.AddSaveRules();
        }
    }

    public class EducationViewModelServerValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelServerValidator()
        {
            this.AddMandatoryRules();
            this.AddSaveRules();
        }
    }

    public class EducationViewModelSaveValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelSaveValidator()
        {
            this.AddSaveRules();
        }
    }

    internal static class EducationValidationRules
    {
        internal static void AddMandatoryRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .NotEmpty()
                .WithMessage(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.RequiredErrorText);

            validator.RuleFor(x => x.FromYear)
                .InclusiveBetween(DateTime.Now.Year - 100, DateTime.Now.Year)
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RangeErrorText, DateTime.Now.Year - 100, DateTime.Now.Year))
                .NotEmpty()
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RequiredErrorText, DateTime.Now.Year - 100, DateTime.Now.Year));

            validator.RuleFor(x => x.ToYear)
                .InclusiveBetween(DateTime.Now.Year - 100, DateTime.Now.Year)
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RangeErrorText, DateTime.Now.Year - 100, DateTime.Now.Year))
                .NotEmpty()
                .WithMessage(string.Format(DateViewModelMessages.YearMessages.RequiredErrorText, DateTime.Now.Year - 100, DateTime.Now.Year));
        }

        internal static void AddSaveRules(this AbstractValidator<EducationViewModel> validator)
        {
            validator.RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .Length(0, 120)
                .WithMessage(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.TooLongErrorText)
                .Matches(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListRegularExpression)
                .WithMessage(EducationViewModelMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListErrorText);

            validator.RuleFor(x => x.FromYear)
                .Must(ValidatorsHelper.BeNowOrInThePast)
                .WithMessage(EducationViewModelMessages.FromYearMessages.NotInFutureErrorText);

            validator.RuleFor(x => x.ToYear)
                  .Must(ValidatorsHelper.EducationYearBeBeforeOrEqual)
                  .WithMessage(EducationViewModelMessages.ToYearMessages.BeforeOrEqualErrorText);
        }
    }
}