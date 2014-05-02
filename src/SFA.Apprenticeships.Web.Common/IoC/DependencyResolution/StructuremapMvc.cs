using Microsoft.Practices.ServiceLocation;
using SFA.Apprenticeships.Web.Common.IoC.DependencyResolution;
using StructureMap;

//[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]

namespace SFA.Apprenticeships.Web.Common.IoC.DependencyResolution
{
    public static class StructuremapMvc
    {
        public static void StartIoC()
        {
            var container = IoC.Initialize();
            var resolver = new StructureMapDependencyResolver(container);

            container.Configure(x =>
            {
                // The object factory container.
                x.For<IContainer>().Use(container);

                // The structure map resolver.
                x.For<IServiceLocator>().Use(resolver);
            });

            // Set the MVC dependency resolver.
            ServiceLocator.SetLocatorProvider(() => resolver);
        }
    }
}