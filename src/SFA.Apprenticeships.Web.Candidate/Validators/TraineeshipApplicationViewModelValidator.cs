namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class TraineeshipApplicationViewModelClientValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelClientValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new TraineeshipViewModelClientValidator());
        }
    }

    public class TraineeshipApplicationViewModelServerValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelServerValidator()
        {
            RuleFor(x => x.Candidate).SetValidator(new TraineeshipViewModelClientValidator());
        }
    }
}