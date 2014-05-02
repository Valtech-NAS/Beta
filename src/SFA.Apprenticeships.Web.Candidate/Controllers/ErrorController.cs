using System.Web.Mvc;
using SFA.Apprenticeships.Web.Common.Models.Errors;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    public class ErrorController : Controller
    {
        //private readonly ILogger _logger;

        public ErrorController()
        {
        }

        //public ErrorController(ILogger logger)
        //{
        //    _logger = logger;
        //}

        [AllowAnonymousAttribute]
        public ActionResult Index()
        {
            ErrorViewModel viewModel = CreateViewModel();
            viewModel.PageTitle = "Error";
            viewModel.WarningMessage = "GenericErrorMessage";

            return View(viewModel);
        }

        [AllowAnonymousAttribute]
        public ActionResult NotFound()
        {
            ErrorViewModel viewModel = CreateViewModel();
            viewModel.PageTitle = "Not found";
            viewModel.WarningMessage = "PageNotFoundMessage";

            return View("index", viewModel);
        }

        [AllowAnonymousAttribute]
        public ActionResult Stop()
        {
            ErrorViewModel viewModel = CreateViewModel();
            viewModel.PageTitle = "AuthenticationAttemptCancelled";
            viewModel.WarningMessage = string.Empty;
            viewModel.ShowCloseButton = true;

            return View("index", viewModel);
        }

        [AllowAnonymousAttribute]
        public ActionResult NotAuthorized()
        {
            //if (_logger != null)
            //{
            //    _logger.Info(
            //        string.Format(
            //            "User {0} does not have permissions to log on",
            //            this.HttpContext.User.Identity.Name));
            //}

            ErrorViewModel viewModel = CreateViewModel();
            viewModel.PageTitle = string.Format("AuthenticationFailedTitle", HttpContext.User.Identity.Name);
            viewModel.WarningMessage = "AuthenticationFailedMessage";
            viewModel.ShowCloseButton = true;

            return View("index", viewModel);
        }

        private ErrorViewModel CreateViewModel()
        {
            return ViewData.Model as ErrorViewModel ?? new ErrorViewModel();
        }
    }
}