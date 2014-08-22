namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Providers;
    using StructureMap;

    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        private IUserDataProvider _userData;
        private IEuCookieDirectiveProvider _euCookieDirectiveProvider;
        private ICookieDetectionProvider _cookieDetectionProvider;

        public IUserDataProvider UserData
        {
            get { return _userData ?? (_userData = new CookieUserDataProvider(HttpContext)); }
        }

        private IEuCookieDirectiveProvider EuCookieDirectiveProvider
        {
            get { return _euCookieDirectiveProvider ?? (_euCookieDirectiveProvider = new EuCookieDirectiveProvider()); }
        }
        
        private ICookieDetectionProvider CookieDetectionProvider
        {
            get { return _cookieDetectionProvider ?? (_cookieDetectionProvider = new CookieDetectionProvider()); }
        }

        private string FeedbackUrl
        {
            get
            {
                // TODO: AG: US475: consider making this 'best efforts' by putting inside a try/catch.
                try
                {
                    var configurationManager = ObjectFactory.GetInstance<IConfigurationManager>();

                    return configurationManager.GetAppSetting<string>("FeedbackUrl");
                }
                catch
                {
                    // CRITICAL: this is best efforts.
                    return "#";
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ShowEuCookieDirective = EuCookieDirectiveProvider.ShowEuCookieDirective(filterContext.HttpContext);
            filterContext.Controller.ViewBag.FeedbackUrl = FeedbackUrl;
            CookieDetectionProvider.SetCookie(filterContext.HttpContext);

            if (!CookieDetectionProvider.IsCookiePresent(filterContext.HttpContext))
            {
                Response.RedirectToRoute(RouteNames.Cookies);
            }

            UserContext = null;

            if (!string.IsNullOrWhiteSpace(User.Identity.Name))
            {
                var context = UserData.GetUserContext();

                if (context != null)
                {
                    var candidateContext = new CandidateUserContext
                    {
                        CandidateId = new Guid(User.Identity.Name),
                        FullName = context.FullName,
                        UserName = context.UserName
                    };

                    UserContext = candidateContext;
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected void SetUserMessage(string message, UserMessageLevel level = UserMessageLevel.Success)
        {
            switch (level)
            {
                case UserMessageLevel.Info:
                    UserData.Push(UserMessageConstants.InfoMessage, message);
                    break;
                case UserMessageLevel.Success:
                    UserData.Push(UserMessageConstants.SuccessMessage, message);
                    break;
                case UserMessageLevel.Warning:
                    UserData.Push(UserMessageConstants.WarningMessage, message);
                    break;
                case UserMessageLevel.Error:
                    UserData.Push(UserMessageConstants.ErrorMessage, message);
                    break;
            }
        }
    }
}
