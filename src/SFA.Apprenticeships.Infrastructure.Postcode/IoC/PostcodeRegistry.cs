namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using System;
    using Application.Interfaces.Locations;
    using Common.Configuration;
    using StructureMap.Configuration.DSL;

    public class PostcodeRegistry : Registry
    {
        public const string PostcodeServiceEndpointAppSetting = "PostcodeServiceEndpoint";

        public PostcodeRegistry()
        {
            For<IPostcodeLookupProvider>()
                .Use<PostcodeService>()
                .Ctor<string>("baseUrl")
                .Is(PostcodeServiceEndpointAppSetting,
                    x =>
                    {
                        var cm = x.GetInstance<IConfigurationManager>();
                        return cm.GetAppSetting(PostcodeServiceEndpointAppSetting);
                    });
        }
    }
}
