namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;

    public class CandidateViewModelValidator : AbstractValidator<CandidateViewModel>
    {
        public CandidateViewModelValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelServerValidator());
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelServerValidator());
        }
    }
}