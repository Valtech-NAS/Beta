namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;

    public class CandidateViewModelClientValidator : AbstractValidator<CandidateViewModel>
    {
        public CandidateViewModelClientValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelClientValidator());
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelClientValidator());

        }
    }

    public class CandidateViewModelServerValidator : AbstractValidator<CandidateViewModel>
    {
        public CandidateViewModelServerValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelServerValidator());
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelServerValidator());
        }
    }
}