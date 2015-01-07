namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;

    public abstract class VacancySearchController : CandidateControllerBase
    {
        protected Guid? GetCandidateId()
        {
            Guid? candidateId = null;

            if (Request.IsAuthenticated && UserContext != null)
            {
                candidateId = UserContext.CandidateId;
            }

            return candidateId;
        }

        protected string GetSearchReturnUrl(string routeName)
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var url = urlHelper.RouteUrl(routeName, null);

            if (Request != null && Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath == url)
                return Request.UrlReferrer.PathAndQuery;

            return null;
        }
    }
}