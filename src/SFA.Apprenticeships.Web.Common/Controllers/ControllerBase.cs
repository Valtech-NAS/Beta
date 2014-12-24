namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Providers;
    using Services;
    using StructureMap.Attributes;

    [AuthenticateUser]
    public abstract class ControllerBase<TContextType> : Controller, IUserController<TContextType> where TContextType : UserContext
    {
        public TContextType UserContext { get; protected set; }

        [SetterProperty]
        public IUserDataProvider UserData { get; set; }

        [SetterProperty]
        public IAuthenticationTicketService AuthenticationTicketService { get; set; }
    }
}
