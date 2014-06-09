namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using StructureMap.Configuration.DSL;

    public class PostcodeRegistry : Registry
    {
        public PostcodeRegistry()
        {
            // TODO::fix this up later          
            For<IPostcodeLookupProvider>().Use<PostcodeService>().Ctor<string>("baseUrl").Is("PostcodeServiceEndpoint", x =>
            {            
                var cm = x.GetInstance<IConfigurationManager>();
                return cm.GetAppSetting("PostcodeServiceEndpoint");
            });

        }
    }
}
