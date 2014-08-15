namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Constants;
    using Providers;

    [AuthenticateUser]
    public abstract class ControllerBase<TContextType> : Controller where TContextType : UserContext
    {
        protected ControllerBase(ISessionStateProvider session)
        {
            Session = session;
        }

        public TContextType UserContext { get; protected set; }

        private new ISessionStateProvider Session { get; set; }

        protected void ClearSession()
        {
            Session.Clear();
        }

        #region Contextual helpers - may move these
        protected void PushContextData<T>(string key, T value)
        {
            //todo: refactor (don't use tempdata! push into context instead or use a non-session store?)
            TempData[key] = value;
        }

        protected T PopContextData<T>(string key)
        {
            //todo: refactor (don't use tempdata! push into context instead or use a non-session store?)
            var value = TempData[key];
            if (value is T) return (T)value;
            return default(T);
        }

        protected void SetUserMessage(string message, UserMessageLevel level = UserMessageLevel.Success)
        {
            //todo: refactor (don't use tempdata! push into context instead or use a non-session store?)
            switch (level)
            {
                case UserMessageLevel.Info:
                    TempData[UserMessageConstants.InfoMessage] = message;
                    break;
                case UserMessageLevel.Success:
                    TempData[UserMessageConstants.SuccessMessage] = message;
                    break;
                case UserMessageLevel.Warning:
                    TempData[UserMessageConstants.WarningMessage] = message;
                    break;
                case UserMessageLevel.Error:
                    TempData[UserMessageConstants.ErrorMessage] = message;
                    break;
            }
        }
        #endregion
    }
}
