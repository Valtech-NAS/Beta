namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;


    public class EmployerQuestionAnswersViewModelClientValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelClientValidator()
        {
            RuleFor(x => x.CandidateAnswer1)
                .NotEmpty()
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.TooLongErrorText)
                .Matches(EmployerQuestionAnswersMessages.CandidateAnswer1.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.WhitelistErrorText);

            RuleFor(x => x.CandidateAnswer2)
                .NotEmpty()
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.TooLongErrorText)
                .Matches(EmployerQuestionAnswersMessages.CandidateAnswer2.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.WhitelistErrorText);
        }
    }

    public class EmployerQuestionAnswersViewModelServerValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelServerValidator()
        {
            RuleFor(x => x.CandidateAnswer1)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion1))
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.TooLongErrorText)
                .Matches(EmployerQuestionAnswersMessages.CandidateAnswer1.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.WhitelistErrorText);

            RuleFor(x => x.CandidateAnswer2)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion2))
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.RequiredErrorText)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.TooLongErrorText)
                .Matches(EmployerQuestionAnswersMessages.CandidateAnswer2.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.WhitelistErrorText);
        }
    }
}