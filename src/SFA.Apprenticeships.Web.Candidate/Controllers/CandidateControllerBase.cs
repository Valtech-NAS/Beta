namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Common.Services;
    using Constants;
    using Providers;
    using StructureMap;

    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        protected CandidateControllerBase()
        {
            //TODO: Think about "new"ing this up instead - Mark doesn't like this. Doesn't need to be lazy, is used everywhere
            UserData = ObjectFactory.GetInstance<IUserDataProvider>();
            
            //TODO: Think about switching these to an action filter set on base controller
            EuCookieDirectiveProvider = ObjectFactory.GetInstance<IEuCookieDirectiveProvider>();
            CookieDetectionProvider = ObjectFactory.GetInstance<ICookieDetectionProvider>();
            AuthenticationTicketService = ObjectFactory.GetInstance<IAuthenticationTicketService>();
            
            var configurationManager = ObjectFactory.GetInstance<IConfigurationManager>();
            FeedbackUrl = configurationManager.TryGetAppSetting("FeedbackUrl") ?? "#";
        }

        public IUserDataProvider UserData { get; set; }

        private ICookieDetectionProvider CookieDetectionProvider { get; set; }

        private IEuCookieDirectiveProvider EuCookieDirectiveProvider { get; set; }

        private IAuthenticationTicketService AuthenticationTicketService { get; set; }

        private string FeedbackUrl { get; set; }

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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (!controllerName.Equals("Login", StringComparison.InvariantCultureIgnoreCase)){
                
                UserData.Pop(UserDataItemNames.SessionReturnUrl);
                var userContext = UserData.GetUserContext();

                if (userContext != null)
                {
                    AddMetaRefreshTimeout();
                }
            }

            AuthenticationTicketService.RefreshTicket(filterContext.Controller.ControllerContext.HttpContext.Response.Cookies);
            base.OnActionExecuted(filterContext);
        }

        private void AddMetaRefreshTimeout()
        {
            var returnUrl = Request != null && Request.Url != null ? Request.Url.PathAndQuery : "/";
            var sessionTimeoutUrl = Url.Action("SessionTimeout", "Login", new { ReturnUrl = returnUrl });
            Response.AppendHeader("Refresh", FormsAuthentication.Timeout.TotalSeconds + ";url=" + sessionTimeoutUrl);
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
