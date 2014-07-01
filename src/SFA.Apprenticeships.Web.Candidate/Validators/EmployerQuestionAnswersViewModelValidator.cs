namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;


    public class EmployerQuestionAnswersViewModelClientValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelClientValidator()
        {
            RuleFor(x => x.CandidateAnswer1)
                .NotEmpty()
                .WithMessage("The employer question must be supplied");

            RuleFor(x => x.CandidateAnswer2)
                .NotEmpty()
                .WithMessage("The employer question must be supplied");
        }
    }

    public class EmployerQuestionAnswersViewModelServerValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelServerValidator()
        {
            RuleFor(x => x.CandidateAnswer1)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion1))
                .WithMessage("The employer question must be supplied");

            RuleFor(x => x.CandidateAnswer2)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion2))
                .WithMessage("The employer question must be supplied");
        }
    }
}