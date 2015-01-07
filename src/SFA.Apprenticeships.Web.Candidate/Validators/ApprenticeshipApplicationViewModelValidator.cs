namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class ApprenticeshipApplicationViewModelClientValidator : AbstractValidator<ApprenticeshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelClientValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelClientValidator());
        }
    }

    public class ApprenticeshipApplicationViewModelServerValidator : AbstractValidator<ApprenticeshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelServerValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelServerValidator());
        }
    }

    public class ApprenticeshipApplicationViewModelSaveValidator : AbstractValidator<ApprenticeshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelSaveValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelSaveValidator());
        }
    }
}