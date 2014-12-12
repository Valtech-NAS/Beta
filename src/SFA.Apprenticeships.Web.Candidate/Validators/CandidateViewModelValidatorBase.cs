namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;

    public abstract class CandidateViewModelClientValidatorBase<T> : AbstractValidator<T> where T : CandidateViewModelBase
    {
        protected CandidateViewModelClientValidatorBase()
        {
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelClientValidator());
        }
    }

    public abstract class CandidateViewModelServerValidator : AbstractValidator<CandidateViewModelBase>
    {
        protected CandidateViewModelServerValidator()
        {
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelServerValidator());
            RuleFor(x => x.Qualifications)
               .SetCollectionValidator(new QualificationViewModelValidator())
               .When(x => x.HasQualifications);
            RuleFor(x => x.WorkExperience)
                .SetCollectionValidator(new WorkExperienceViewModelValidator())
                .When(x => x.HasWorkExperience);
        }
    }
}