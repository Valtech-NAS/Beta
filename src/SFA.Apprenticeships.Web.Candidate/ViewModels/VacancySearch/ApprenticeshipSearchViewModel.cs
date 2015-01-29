namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof (ApprenticeshipSearchViewModelClientValidator))]
    public class ApprenticeshipSearchViewModel : VacancySearchViewModel
    {
        public ApprenticeshipSearchViewModel()
        {

        }

        public ApprenticeshipSearchViewModel(ApprenticeshipSearchViewModel viewModel) : base (viewModel)
        {
            Keywords = viewModel.Keywords;
            LocationType = viewModel.LocationType;
            ApprenticeshipLevel = viewModel.ApprenticeshipLevel;
            Category = viewModel.Category;
            SubCategories = viewModel.SubCategories;
            SearchMode = viewModel.SearchMode;
        }

        [Display(Name = ApprenticeshipSearchViewModelMessages.KeywordMessages.LabelText, Description = ApprenticeshipSearchViewModelMessages.KeywordMessages.HintText)]
        public string Keywords { get; set; }

        [Display(Name = ApprenticeshipSearchViewModelMessages.LocationMessages.LabelText, Description = ApprenticeshipSearchViewModelMessages.LocationMessages.HintText)]
        public override string Location { get; set; }

        public ApprenticeshipLocationType LocationType { get; set; }

        public ApprenticeshipSearchViewModel[] LocationSearches { get; set; }

        public SelectList ApprenticeshipLevels { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public IList<Category> Categories { get; set; }

        public string Category { get; set; }

        public string[] SubCategories { get; set; }

        public ApprenticeshipSearchMode SearchMode { get; set; }
    }
}
