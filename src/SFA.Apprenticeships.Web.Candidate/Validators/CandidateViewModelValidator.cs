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
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelServerValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelServerValidator());
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelServerValidator());
            RuleFor(x => x.Qualifications)
               .SetCollectionValidator(new QualificationViewModelValidator())
               .When(x => x.HasQualifications);
            RuleFor(x => x.WorkExperience)
                .SetCollectionValidator(new WorkExperienceViewModelValidator())
                .When(x => x.HasWorkExperience);
        }
    }

    public class CandidateViewModelSaveValidator : AbstractValidator<CandidateViewModel>
    {
        public CandidateViewModelSaveValidator()
        {
            RuleFor(x => x.AboutYou).SetValidator(new AboutYouViewModelSaveValidator());
            RuleFor(x => x.Education).SetValidator(new EducationViewModelSaveValidator());
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelSaveValidator());
            RuleFor(x => x.Qualifications)
                .SetCollectionValidator(new QualificationViewModelValidator())
                .When(x => x.HasQualifications);
            RuleFor(x => x.WorkExperience)
                .SetCollectionValidator(new WorkExperienceViewModelValidator())
                .When(x => x.HasWorkExperience);
        }
    }
}