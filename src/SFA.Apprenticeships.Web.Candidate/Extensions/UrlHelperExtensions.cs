namespace SFA.Apprenticeships.Web.Candidate.Extensions
{
    using System.Web.Mvc;
    using ViewModels.VacancySearch;

    public static class UrlHelperExtensions
    {
        public static string ApprenticeshipSearchViewModelAction(this UrlHelper url, string actionName, ApprenticeshipSearchViewModel model)
        {
            var subCategories = model.SubCategories;
            model.SubCategories = null;
            var actionUrl = url.Action(actionName, model) + subCategories.ToQueryString("SubCategories");
            model.SubCategories = subCategories;
            return actionUrl;
        }
    }
}