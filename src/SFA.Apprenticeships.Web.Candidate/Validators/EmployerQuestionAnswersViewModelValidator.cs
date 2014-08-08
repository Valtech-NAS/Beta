namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;


    public class EmployerQuestionAnswersViewModelClientValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class EmployerQuestionAnswersViewModelServerValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public class EmployerQuestionAnswersViewModelSaveValidator : AbstractValidator<EmployerQuestionAnswersViewModel>
    {
        public EmployerQuestionAnswersViewModelSaveValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class EmployerQuestionValidaitonRules
    {
        internal static void AddCommonRules(this AbstractValidator<EmployerQuestionAnswersViewModel> validator)
        {
            validator.RuleFor(x => x.CandidateAnswer1)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.TooLongErrorText)
                .Matches(EmployerQuestionAnswersMessages.CandidateAnswer1.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.WhitelistErrorText);

            validator.RuleFor(x => x.CandidateAnswer2)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.TooLongErrorText)
                .Matches(EmployerQuestionAnswersMessages.CandidateAnswer2.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.WhitelistErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<EmployerQuestionAnswersViewModel> validator)
        {
            validator.RuleFor(x => x.CandidateAnswer1)
                .NotEmpty()
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.RequiredErrorText);

            validator.RuleFor(x => x.CandidateAnswer2)
                .NotEmpty()
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<EmployerQuestionAnswersViewModel> validator)
        {
            validator.RuleFor(x => x.CandidateAnswer1)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion1))
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer1.RequiredErrorText);

            validator.RuleFor(x => x.CandidateAnswer2)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion2))
                .WithMessage(EmployerQuestionAnswersMessages.CandidateAnswer2.RequiredErrorText);
        }
    }
}