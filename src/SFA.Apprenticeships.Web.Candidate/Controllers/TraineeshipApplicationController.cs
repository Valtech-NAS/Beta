namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Web.Candidate.ActionResults;
    using SFA.Apprenticeships.Web.Candidate.Attributes;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Common.Constants;

    public class TraineeshipApplicationController : CandidateControllerBase
    {
        //[OutputCache(CacheProfile = CacheProfiles.None)]
        //[AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        //[ApplyWebTrends]
        //public async Task<ActionResult> Apply(int id)
        //{
        //    return await Task.Run<ActionResult>(() =>
        //    {
        //        //var model = _applicationProvider.GetApplicationViewModel(UserContext.CandidateId, id);

        //        //if (model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
        //        //{
        //        //    return new VacancyNotFoundResult();
        //        //}

        //        //if (model.HasError())
        //        //{
        //        //    return RedirectToRoute(CandidateRouteNames.MyApplications);
        //        //}

        //        //model.SessionTimeout = FormsAuthentication.Timeout.TotalSeconds - 30;

        //        var model = 

        //        return View(model);
        //    });
        //}
    }
}