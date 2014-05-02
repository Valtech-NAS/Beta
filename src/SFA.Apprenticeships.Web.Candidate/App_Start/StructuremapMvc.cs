using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using SFA.Apprenticeships.Web.Candidate;
using SFA.Apprenticeships.Web.Common.IoC.DependencyResolution;
using StructureMap;
using StructureMap.Graph;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]

namespace SFA.Apprenticeships.Web.Candidate
{
    /// <summary>
    /// StructureMap MVC initialization. Sets the MVC resolver and the WebApi resolver to use structure map.
    /// </summary>
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = IoC.Initialize();
            var resolver = new StructureMapDependencyResolver(container);

            container.Configure(x =>
            {
                // The object factory container.
                x.For<IContainer>().Use(container);

                // The structure map resolver.
                x.For<IServiceLocator>().Use(resolver);

                x.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.WithDefaultConventions();
                });
            });

            // Set the MVC/WebApi dependency resolver.
            ServiceLocator.SetLocatorProvider(() => resolver);
            DependencyResolver.SetResolver(ServiceLocator.Current);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}