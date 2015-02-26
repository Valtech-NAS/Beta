namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Web.Routing;
    using Candidate.ViewModels.VacancySearch;
    using Candidate.Views.ApprenticeshipSearch;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class SearchResultsViewBuilder
    {
        private ApprenticeshipSearchResponseViewModel _viewModel = new ApprenticeshipSearchResponseViewModel();

        public SearchResultsViewBuilder()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public SearchResultsViewBuilder With(ApprenticeshipSearchResponseViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public searchResults Build()
        {
            var view = new searchResults { ViewData = { Model = _viewModel } };
            return view;
        }

        public HtmlDocument Render()
        {
            var view = Build();
            var result = view.RenderAsHtml(_viewModel);
            return result;
        }
    }
}