namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Controllers;
    using Infrastructure.Logging;
    using NLog;
    using Providers;

    [SessionTimeout, CookiesEnabled, AllowReturnUrl(Allow = true), ClearSearchReturnUrl, PlannedOutageMessage]
    public abstract class CandidateControllerBase : ControllerBase<CandidateUserContext>
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
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
                        UserName = context.UserName,
                        AcceptedTermsAndConditionsVersion = context.AcceptedTermsAndConditionsVersion
                    };

                    UserContext = candidateContext;
                }
            }

            SetLoggingIds();

            SetAbout();

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

        private void SetAbout()
        {
            var showAbout = bool.Parse(ConfigurationManager.AppSettings["ShowAbout"]);

            ViewBag.ShowAbout = showAbout;

            if (!showAbout) return;

            ViewBag.Version = VersionLogging.GetVersion();
            ViewBag.Environment = ConfigurationManager.AppSettings["Environment"];
        }

        private void SetLoggingIds()
        {
            var sessionId = UserData.Get(UserDataItemNames.LoggingSessionId);
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString("N");
                UserData.Push(UserDataItemNames.LoggingSessionId, sessionId);
            }

            MappedDiagnosticsContext.Set("sessionId", sessionId);
            MappedDiagnosticsContext.Set("userId", UserContext != null ? UserContext.CandidateId.ToString() : "<none>");
        }

        protected Guid? GetCandidateId()
        {
            Guid? candidateId = null;

            if (Request.IsAuthenticated && UserContext != null)
            {
                candidateId = UserContext.CandidateId;
            }

            return candidateId;
        }
    }
}
