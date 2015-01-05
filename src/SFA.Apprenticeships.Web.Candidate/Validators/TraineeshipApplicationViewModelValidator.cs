namespace SFA.Apprenticeships.Web.Candidate.Validators
{
    using FluentValidation;
    using ViewModels.Applications;

    public class TraineeshipApplicationViewModelClientValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelClientValidator()
        {
//TODO: review
#pragma warning disable 618 
            RuleFor(x => x.Candidate).SetValidator(new TraineeshipViewModelClientValidator());
#pragma warning restore 618
        }
    }

    public class TraineeshipApplicationViewModelServerValidator : AbstractValidator<TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModelServerValidator()
        {
//TODO: review
#pragma warning disable 618
            RuleFor(x => x.Candidate).SetValidator(new TraineeshipViewModelClientValidator());
#pragma warning restore 618
        }
    }
}