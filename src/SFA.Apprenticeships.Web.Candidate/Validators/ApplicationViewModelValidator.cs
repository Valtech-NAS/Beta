namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class ApplicationViewModelClientValidator : AbstractValidator<ApplicationViewModel>
    {
        public ApplicationViewModelClientValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new CandidateViewModelClientValidator());
        }
    }

    public class ApplicationViewModelServerValidator : AbstractValidator<ApplicationViewModel>
    {
        public ApplicationViewModelServerValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new CandidateViewModelServerValidator());
        }
    }
}