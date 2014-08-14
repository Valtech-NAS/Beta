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
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer1Messages.TooLongErrorText)
                .Matches(EmployerQuestionAnswersViewModelMessages.CandidateAnswer1Messages.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer1Messages.WhitelistErrorText);

            validator.RuleFor(x => x.CandidateAnswer2)
                .Length(0, 4000)
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer2Messages.TooLongErrorText)
                .Matches(EmployerQuestionAnswersViewModelMessages.CandidateAnswer2Messages.WhitelistRegularExpression)
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer2Messages.WhitelistErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<EmployerQuestionAnswersViewModel> validator)
        {
            validator.RuleFor(x => x.CandidateAnswer1)
                .NotEmpty()
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer1Messages.RequiredErrorText);

            validator.RuleFor(x => x.CandidateAnswer2)
                .NotEmpty()
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer2Messages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<EmployerQuestionAnswersViewModel> validator)
        {
            validator.RuleFor(x => x.CandidateAnswer1)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion1))
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer1Messages.RequiredErrorText);

            validator.RuleFor(x => x.CandidateAnswer2)
                .NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.SupplementaryQuestion2))
                .WithMessage(EmployerQuestionAnswersViewModelMessages.CandidateAnswer2Messages.RequiredErrorText);
        }
    }
}