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
            RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .Length(0, 120)
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.RequiredErrorText)
                .Matches(EducationMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListErrorText);

            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(EducationMessages.FromYearMessages.RequiredErrorText)
                .Matches(EducationMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.FromYearMessages.WhiteListErrorText);

            RuleFor(x => x.ToYear)
                .NotEmpty()
                .WithMessage(EducationMessages.ToYearMessages.RequiredErrorText)
                .Matches(EducationMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.FromYearMessages.WhiteListErrorText);
        }
    }

    public class EducationViewModelServerValidator : AbstractValidator<EducationViewModel>
    {
        public EducationViewModelServerValidator()
        {
            RuleFor(x => x.NameOfMostRecentSchoolCollege)
                .Length(0, 120)
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.RequiredErrorText)
                .Matches(EducationMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.NameOfMostRecentSchoolCollegeMessages.WhiteListErrorText);

            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(EducationMessages.FromYearMessages.RequiredErrorText)
                .Matches(EducationMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.FromYearMessages.WhiteListErrorText);

            RuleFor(x => x.ToYear)
                .NotEmpty()
                .WithMessage(EducationMessages.ToYearMessages.RequiredErrorText)
                .Matches(EducationMessages.ToYearMessages.WhiteListRegularExpression)
                .WithMessage(EducationMessages.ToYearMessages.WhiteListErrorText)
                .Must(BeBeforeOrEqual)
                .WithMessage(EducationMessages.ToYearMessages.BeforeOrEqualErrorText);
        }

        private bool BeBeforeOrEqual(EducationViewModel instance, string toYear)
        {
            int to = int.Parse(toYear);
            int from = int.Parse(instance.FromYear);
            return from <= to;
        }
    }

}