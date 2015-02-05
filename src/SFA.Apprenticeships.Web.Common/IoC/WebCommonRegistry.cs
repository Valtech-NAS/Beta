namespace SFA.Apprenticeships.Web.Common.IoC
{
    using Services;
    using StructureMap.Configuration.DSL;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IAuthenticationTicketService>().Use<AuthenticationTicketService>();
        }
    }
}