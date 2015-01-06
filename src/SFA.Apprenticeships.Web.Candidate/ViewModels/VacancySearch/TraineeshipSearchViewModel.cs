namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.ComponentModel.DataAnnotations;
    using Application.Interfaces.Vacancies;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(TraineeshipSearchViewModelClientValidator))]
    public class TraineeshipSearchViewModel : VacancySearchViewModel
    {
        public TraineeshipSearchViewModel()
        {
            SortType = VacancySortType.Distance;
        }

        public TraineeshipSearchViewModel(TraineeshipSearchViewModel viewModel) : base(viewModel)
        {
        }

        [Display(Name = TraineeshipSearchViewModelMessages.LocationMessages.LabelText, Description = TraineeshipSearchViewModelMessages.LocationMessages.HintText)]
        public override string Location { get; set; }

        public TraineeshipSearchViewModel[] LocationSearches { get; set; }
    }
}
