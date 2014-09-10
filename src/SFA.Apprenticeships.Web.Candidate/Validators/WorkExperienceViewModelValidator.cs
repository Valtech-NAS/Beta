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
                .Length(0, 200)
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.DescriptionMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.DescriptionMessages.WhiteListErrorText);

            RuleFor(x => x.Employer)
                .Length(0, 50)
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.EmployerMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.EmployerMessages.WhiteListErrorText);

            RuleFor(x => x.JobTitle)
                .Length(0, 50)
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.JobTitleMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.JobTitleMessages.WhiteListErrorText);

            RuleFor(x => x.FromYear)
                .NotEmpty()
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.RequiredErrorText)
                .Matches(WorkExperienceViewModelMessages.FromYearMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.WhiteListErrorText)
                .Must(BeNowOrInThePast)
                .WithMessage(WorkExperienceViewModelMessages.FromYearMessages.BeforeOrEqualErrorText);

            RuleFor(x => x.ToYear)
                .Must(BeNowOrInThePast)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.BeforeOrEqualErrorText)
                .Matches(WorkExperienceViewModelMessages.ToYearMessages.WhiteListRegularExpression)
                .WithMessage(WorkExperienceViewModelMessages.ToYearMessages.WhiteListErrorText);
        }     

        private static bool BeNowOrInThePast( string year)
        {
            if (string.IsNullOrEmpty(year))
            {
                //Will be picked up by required validator
                return true;
            }
            var from = int.Parse(year);
            return from <= DateTime.Now.Year;
        }
    }
}