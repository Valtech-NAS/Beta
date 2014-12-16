namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Domain.Interfaces.Configuration;
    using StructureMap.Attributes;

    public class TraineeshipsToggle : ActionFilterAttribute
    {
        [SetterProperty]
        public IFeatureToggle FeatureToggle { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!FeatureToggle.IsActive(Feature.Traineeships))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}