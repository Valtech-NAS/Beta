namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Providers;

    [AuthenticateUser]
    public abstract class ControllerBase<TContextType> : Controller where TContextType : UserContext
    {
        public TContextType UserContext { get; protected set; } //todo: may move...

        private new ISessionStateProvider Session
        {
            get { throw new InvalidOperationException("Use of session is not supported"); }
        }

        protected void ClearSession()
        {
            base.Session.Clear();
        }
    }
}
