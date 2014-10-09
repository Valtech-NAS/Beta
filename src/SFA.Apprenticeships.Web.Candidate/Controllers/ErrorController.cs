namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Runtime.Remoting.Messaging;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Constants;

    public class ErrorController : Controller
    {
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> NotFound()
        {
            return await Task.Run<ActionResult>(() => View("NotFound"));
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> InternalServerError()
        {
            return await Task.Run<ActionResult>(() =>  View("InternalServerError"));
        }
    }
}
