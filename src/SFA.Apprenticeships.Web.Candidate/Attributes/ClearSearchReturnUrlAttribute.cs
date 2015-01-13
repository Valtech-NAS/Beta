namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System.Web.Mvc;
    using Controllers;

    public class ClearSearchReturnUrlAttribute : ActionFilterAttribute
    {
        const string SearchReturnUrlKey = "SearchReturnUrl";

        public ClearSearchReturnUrlAttribute()
        {
            ClearSearchReturnUrl = true;
        }

        public bool ClearSearchReturnUrl { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var candidateController = filterContext.Controller as CandidateControllerBase;

            if (candidateController != null && candidateController.UserData.Get(SearchReturnUrlKey) != null && candidateController.ViewBag.SearchReturnUrl == null)
            {
                candidateController.ViewBag.SearchReturnUrl = candidateController.UserData.Pop(SearchReturnUrlKey);
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var candidateController = filterContext.Controller as CandidateControllerBase;

            if (!ClearSearchReturnUrl && candidateController != null && candidateController.ViewBag.SearchReturnUrl != null)
            {
                candidateController.UserData.Push(SearchReturnUrlKey, candidateController.ViewBag.SearchReturnUrl);
            }

            base.OnResultExecuted(filterContext);
        }
    }
}