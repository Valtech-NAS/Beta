using System.Web.Mvc;
using SFA.Apprenticeships.Web.Common.Providers;
using StructureMap;
using StructureMap.Graph;

namespace SFA.Apprenticeships.Web.Common.IoC.DependencyResolution
{
    /// <summary>
    /// IoC Container
    /// </summary>
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = ObjectFactory.Container;
            ObjectFactory.Initialize(
                x => x.Scan(
                    scan =>
                    {
                        scan.TheCallingAssembly();
                        scan.WithDefaultConventions();
                    }));

            return container
                .LoadCommonConfiguration()
                .LoadWebConfiguration();
        }

        private static IContainer LoadWebConfiguration(this IContainer container)
        {
            container.Configure(x =>
            {
                //x.For<HttpSessionStateBase>().Use(() => new HttpSessionStateWrapper(HttpContext.Current.Session));
            });

            return container;
        }

        private static IContainer LoadCommonConfiguration(this IContainer container)
        {
            container.Configure(x =>
            {
                x.For<IReferenceDataProvider>().Use<ConfigReferenceDataProvider>();
                //x.For<HttpSessionStateBase>().Use(() => new HttpSessionStateWrapper(HttpContext.Current.Session));
            });

            return container;
        }
    }
}