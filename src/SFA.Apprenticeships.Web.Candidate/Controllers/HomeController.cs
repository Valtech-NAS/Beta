namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Constants;

    public class HomeController : CandidateControllerBase
    {
        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> Privacy()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> Cookies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return await Task.Run<ActionResult>(() => View());
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> Helpdesk()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> Terms()
        {
            return await Task.Run<ActionResult>(() => View());
        }
    }
}