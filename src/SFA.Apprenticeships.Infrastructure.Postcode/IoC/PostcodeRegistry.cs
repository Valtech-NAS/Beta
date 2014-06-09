namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using StructureMap.Configuration.DSL;

    public class PostcodeRegistry : Registry
    {
        public const string PostcodeServiceEndpointAppSetting = "PostcodeServiceEndpoint";

        public PostcodeRegistry()
        {
            For<IPostcodeLookupProvider>()
                .Use<PostcodeService>()
                .Ctor<string>("baseUrl")
                .Is(PostcodeServiceEndpointAppSetting, x =>
                {
                    var cm = x.GetInstance<IConfigurationManager>();
                    return cm.GetAppSetting(PostcodeServiceEndpointAppSetting);
                });

        }
    }
}