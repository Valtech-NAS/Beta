namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using Providers;
    using Services;

    public interface IUserContoller<out TContextType> : IUserContoller where TContextType : UserContext
    {
        TContextType UserContext { get; }
    }

    public interface IUserContoller 
    {
        IUserDataProvider UserData { get; }

        IAuthenticationTicketService AuthenticationTicketService { get; }
    }
}
