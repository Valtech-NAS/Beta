namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(TraineeshipSearchViewModelClientValidator))]
    public class TraineeshipSearchViewModel : VacancySearchViewModel
    {
        public TraineeshipSearchViewModel() 
        {
        }

        public TraineeshipSearchViewModel(TraineeshipSearchViewModel viewModel) : base(viewModel)
        {
        }

        [Display(Name = TraineeshipSearchViewModelMessages.LocationMessages.LabelText, Description = TraineeshipSearchViewModelMessages.LocationMessages.HintText)]
        public override string Location { get; set; }
    }
}
