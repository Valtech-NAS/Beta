namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class ApprenticeshipApplicationViewModelClientValidator : AbstractValidator<ApprenticheshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelClientValidator()
        {
//TODO: review
#pragma warning disable 618 
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelClientValidator());
#pragma warning restore 618
        }
    }

    public class ApprenticeshipApplicationViewModelServerValidator : AbstractValidator<ApprenticheshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelServerValidator()
        {
//TODO: review
#pragma warning disable 618
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelServerValidator());
#pragma warning restore 618
        }
    }

    public class ApprenticeshipApplicationViewModelSaveValidator : AbstractValidator<ApprenticheshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModelSaveValidator()
        {
//TODO: review
#pragma warning disable 618
            RuleFor(x => x.Candidate).SetValidator(new ApprenticeshipViewModelSaveValidator());
#pragma warning restore 618
        }
    }
}