namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class ApplicationViewModelValidator : AbstractValidator<ApplicationViewModel>
    {
        public ApplicationViewModelValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new CandidateViewModelValidator());
        }
    }
}