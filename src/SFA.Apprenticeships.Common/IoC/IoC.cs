using System.Linq;

namespace SFA.Apprenticeships.Common.IoC
{
    using StructureMap;

    /// <summary>
    /// IoC Container
    /// </summary>
    public static class IoC
    {
        public static IContainer Initialize(RegisterAssembly[] registryAssemblies)
        {
            ObjectFactory.Initialize(
                x => x.Scan(
                    scan =>
                    {
                        foreach (var registryAssembly in registryAssemblies.OrderBy(p => p.Priority))
                        {
                            scan.Assembly(registryAssembly.Name);
                        }

                        scan.LookForRegistries();
                    }));

            return ObjectFactory.Container;
        }
    }
}