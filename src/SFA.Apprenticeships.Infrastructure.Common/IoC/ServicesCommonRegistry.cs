namespace SFA.Apprenticeships.Infrastructure.Common.IoC
{
    using SFA.Apprenticeships.Infrastructure.Common.ActiveDirectory;
    using StructureMap.Configuration.DSL;

    public class ServicesCommonRegistry : Registry
    {
        public ServicesCommonRegistry()
        {
            For<IActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
        }
    }
}
