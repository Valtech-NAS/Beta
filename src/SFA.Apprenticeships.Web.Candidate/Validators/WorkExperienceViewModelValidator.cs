namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using System;
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class WorkExperienceViewModelValidator : AbstractValidator<WorkExperienceViewModel>
    {
        public WorkExperienceViewModelValidator()
        {
            RuleFor(x => x.Description)
                .Length(0, 4000)
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.DescriptionMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.WhiteListErrorText);

            RuleFor(x => x.Employer)
                .Length(0, 200)
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.EmployerMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.WhiteListErrorText);

            RuleFor(x => x.JobTitle)
                .Length(0, 4000)
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.JobTitleMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.WhiteListErrorText);

            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.RequiredErrorText)
                .GreaterThanOrEqualTo(0)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.MustBeNumericText)
                .InclusiveBetween(DateTime.Now.Year - 100, DateTime.Now.Year)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.BeforeOrEqualErrorText);

            RuleFor(x => x.ToYear)
                .GreaterThanOrEqualTo(0)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.MustBeNumericText)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.BeforeOrEqualErrorText)
                .Must(MustBeZeroOrFourDigitNumber)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.MustBeFourDigitNumberErrorText);
        }

        private bool MustBeZeroOrFourDigitNumber(int year)
        {
            var dateTimeNow = DateTime.Now;
            return year == 0 || year > dateTimeNow.Year - 100;
        }
    }
}