namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Attributes;
    using Constants;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Home;
    using ViewModels.Home;

    public class HomeController : CandidateControllerBase
    {
        private readonly IHomeMediator _homeMediator;

        public HomeController(IHomeMediator homeMediator)
        {
            _homeMediator = homeMediator;
        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        [SiteRootRedirect]
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
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var response = _homeMediator.GetContactMessageViewModel(candidateId);
                return View(response.ViewModel);
            });
        }

        [HttpPost]
        [OutputCache(CacheProfile = CacheProfiles.None)]
        [ApplyWebTrends]
        public async Task<ActionResult> Helpdesk(ContactMessageViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();
                var response = _homeMediator.SendContactMessage(candidateId, model);

                switch (response.Code)
                {
                    case HomeMediatorCodes.SendContactMessage.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(model);
                    case HomeMediatorCodes.SendContactMessage.SuccessfullySent:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case HomeMediatorCodes.SendContactMessage.Error:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });

        }

        [OutputCache(CacheProfile = CacheProfiles.Long)]
        [ApplyWebTrends]
        public async Task<ActionResult> Terms()
        {
            return await Task.Run<ActionResult>(() => View());
        }
    }
}