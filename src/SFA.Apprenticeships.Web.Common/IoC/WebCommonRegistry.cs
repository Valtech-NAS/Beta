namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Web.Http;
    using System.Web.Mvc;
    using CuttingEdge.Conditions;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.ReferenceData;
    using Infrastructure.LegacyWebServices.ReferenceData;
    using StructureMap.Configuration.DSL.Expressions;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();
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
