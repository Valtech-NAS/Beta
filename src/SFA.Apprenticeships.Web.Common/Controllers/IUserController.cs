namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using Providers;
    using Services;

    public interface IUserController<out TContextType> : IUserController where TContextType : UserContext
    {
        TContextType UserContext { get; }
    }

    public interface IUserController 
    {
        IUserDataProvider UserData { get; }

        IAuthenticationTicketService AuthenticationTicketService { get; }
    }
}
