namespace SFA.Apprenticeships.Infrastructure.Azure.Session.IoC
{
    using System.Web;
    using StructureMap.Configuration.DSL;

    public class SessionRegistry : Registry
    {
        public SessionRegistry()
        {
            For<ISessionState>().Use<AzureSessionState>();
            For<HttpSessionStateBase>().Use(ctx => new HttpSessionStateWrapper(HttpContext.Current.Session));
        }
    }
}
