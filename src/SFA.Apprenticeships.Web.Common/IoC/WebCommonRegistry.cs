namespace SFA.Apprenticeships.Web.Common.IoC
{
    using System.Web.Http;
    using System.Web.Mvc;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.ReferenceData;
    using Infrastructure.LegacyWebServices.ReferenceData;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();

            var resolver = new StructureMapDependencyResolver(ObjectFactory.Container);

            For<IServiceLocator>().Use(resolver);
            For<IContainer>().Use(ObjectFactory.Container);

            // Set the MVC/WebApi dependency resolver.
            ServiceLocator.SetLocatorProvider(() => resolver);
            DependencyResolver.SetResolver(ServiceLocator.Current);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
