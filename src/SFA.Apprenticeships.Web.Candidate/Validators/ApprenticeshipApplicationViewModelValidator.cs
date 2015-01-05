namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class ApprenticeshipApplicationViewModelClientValidator : AbstractValidator<ApprenticheshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelClientValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelClientValidator());
        }
    }

    public class ApprenticeshipApplicationViewModelServerValidator : AbstractValidator<ApprenticheshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelServerValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelServerValidator());
        }
    }

    public class ApprenticeshipApplicationViewModelSaveValidator : AbstractValidator<ApprenticheshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelSaveValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelSaveValidator());
        }
    }
}