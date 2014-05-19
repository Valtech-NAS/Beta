namespace SFA.Apprenticeships.Common.IoC
{
    using StructureMap;

    /// <summary>
    /// IoC Container
    /// </summary>
    public static class IoC
    {
        public static IContainer Initialize(string[] registryAssemblies)
        {
            var container = ObjectFactory.Container;
            ObjectFactory.Initialize(
                x => x.Scan(
                    scan =>
                    {
                        foreach (var registryAssembly in registryAssemblies)
                        {
                            scan.Assembly(registryAssembly);
                        }
                        scan.LookForRegistries();
                    }));

            return container;
        }
    }
}