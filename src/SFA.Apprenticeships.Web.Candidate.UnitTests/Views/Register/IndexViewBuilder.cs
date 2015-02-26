namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Register
{
    using System.Web.Routing;
    using Candidate.ViewModels.Register;
    using Candidate.Views.Register;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class IndexViewBuilder
    {
        private RegisterViewModel _viewModel;

        public IndexViewBuilder()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public IndexViewBuilder With(RegisterViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public Index Build()
        {
            var view = new Index { ViewData = { Model = _viewModel } };
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