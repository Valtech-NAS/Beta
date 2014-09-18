namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System;
    using System.Web.Mvc;
    using Attributes;
    using Providers;
    using Services;

    [AuthenticateUser]
    public abstract class ControllerBase<TContextType> : Controller, IUserController<TContextType> where TContextType : UserContext
    {
        public TContextType UserContext { get; protected set; }

        public IUserDataProvider UserData { get; protected set; }

        public IAuthenticationTicketService AuthenticationTicketService  { get; protected set; }
    }
}
