namespace SFA.Apprenticeships.Infrastructure.Azure.Session.IoC
{
    using System.Web;
    using StructureMap.Configuration.DSL;
    using Web.Common.Providers;

    public class SessionRegistry : Registry
    {
        public SessionRegistry()
        {
            For<ISessionStateProvider>().Use<AzureSessionState>();
            For<HttpSessionStateBase>().Use(ctx => new HttpSessionStateWrapper(HttpContext.Current.Session));
        }
    }
}
