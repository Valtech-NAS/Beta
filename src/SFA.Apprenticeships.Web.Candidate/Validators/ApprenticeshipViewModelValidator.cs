namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;

    public class ApprenticeshipViewModelClientValidator : CandidateViewModelClientValidatorBase<ApprenticeshipCandidateViewModel>
    {
        public ApprenticeshipViewModelClientValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelClientValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelClientValidator());
        }
    }

    public class ApprenticeshipViewModelServerValidator : CandidateViewModelServerValidatorBase<ApprenticeshipCandidateViewModel>
    {
        public ApprenticeshipViewModelServerValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelServerValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelServerValidator());
        }
    }

    public class ApprenticeshipViewModelSaveValidator : AbstractValidator<ApprenticeshipCandidateViewModel>
    {
        public ApprenticeshipViewModelSaveValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelSaveValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelSaveValidator());
        }
    }
}