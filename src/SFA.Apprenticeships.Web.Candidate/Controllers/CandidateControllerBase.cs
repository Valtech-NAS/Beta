namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Web.Mvc;
    using Common.Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Common.Providers;
    using Common.Services;
    using Domain.Interfaces.Configuration;
    using Providers;
    using StructureMap;

    [SessionTimeout, CookiesEnabled, AllowReturnUrl(Allow = true)]
    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        protected CandidateControllerBase()
        {
            //TODO: Think about "new"ing this up instead - Mark doesn't like this. Doesn't need to be lazy, is used everywhere
            //TODO: VGA: can't we inject them? It's very difficult to test the controllers if not.
            UserData = ObjectFactory.GetInstance<IUserDataProvider>();
            AuthenticationTicketService = ObjectFactory.GetInstance<IAuthenticationTicketService>();

            var configurationManager = ObjectFactory.GetInstance<IConfigurationManager>();
            FeedbackUrl = configurationManager.TryGetAppSetting("FeedbackUrl") ?? "#";
        }

        private string FeedbackUrl { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.FeedbackUrl = FeedbackUrl;

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

            SetLoggingIds();

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

        private void SetLoggingIds()
        {
            var sessionId = UserData.Get(UserDataItemNames.LoggingSessionId);
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString("N");
                UserData.Push(UserDataItemNames.LoggingSessionId, sessionId);
            }

            NLog.MappedDiagnosticsContext.Set("sessionId", sessionId);
            NLog.MappedDiagnosticsContext.Set("userId", UserContext != null ? UserContext.CandidateId.ToString() : "<none>");
        }
    }
}
