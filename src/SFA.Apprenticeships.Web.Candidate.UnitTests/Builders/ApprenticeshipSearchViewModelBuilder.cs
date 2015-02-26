namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.VacancySearch;

    public class ApprenticeshipSearchViewModelBuilder
    {
        public ApprenticeshipSearchViewModel Build()
        {
            var viewModel = new ApprenticeshipSearchViewModel
            {
                SortTypes = SearchMediatorBase.GetSortTypes(),
                ResultsPerPageSelectList = SearchMediatorBase.GetResultsPerPageSelectList(5)
            };
            return viewModel;
        }
    }
}