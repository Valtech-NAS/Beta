namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Web.Http;
    using System.Web.Mvc;
    using Application.Interfaces.ReferenceData;
    using CuttingEdge.Conditions;
    using Infrastructure.LegacyWebServices.ReferenceData;
    using Microsoft.Practices.ServiceLocation;
    using Services;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();
            For<IAuthenticationTicketService>().Use<AuthenticationTicketService>();
        }

        public static void Configure(IContainer container)
        {
            Condition.Requires(container, "container").IsNotNull();

            var resolver = new StructureMapDependencyResolver(container);

            container.Configure(x =>
            {
                x.For<IContainer>().Use(container);
                x.For<IServiceLocator>().Use(resolver);
            });

            // Set the MVC/WebApi dependency resolver.
            ServiceLocator.SetLocatorProvider(() => resolver);
            DependencyResolver.SetResolver(ServiceLocator.Current);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
