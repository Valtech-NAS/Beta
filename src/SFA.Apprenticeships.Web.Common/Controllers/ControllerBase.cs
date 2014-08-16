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
            Session = session; //todo: remove
        }

        public TContextType UserContext { get; protected set; } ///todo: may move... stay here only if used in common

        private new ISessionStateProvider Session { get; set; } //todo: remove and throw ex if try to use Session!

        protected void ClearSession()
        {
            Session.Clear(); //todo: base.Session.Clear();
        }

        #region Contextual helpers - may move these
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
