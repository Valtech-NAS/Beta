namespace SFA.Apprenticeships.Services.Common.IoC
{
    using SFA.Apprenticeships.Services.Common.ActiveDirectory;
    using StructureMap.Configuration.DSL;

    public class ServicesCommonRegistry : Registry
    {
        public ServicesCommonRegistry()
        {
            For<IActiveDirectoryConfiguration>().Singleton().Use(ActiveDirectoryConfiguration.Instance);
        }
    }
}
