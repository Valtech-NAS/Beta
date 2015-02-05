namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Configuration;

    public class SavedSearchesToggle : ActionFilterAttribute
    {
        public IFeatureToggle FeatureToggle { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!FeatureToggle.IsActive(Feature.SavedSearches))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}