using SFA.Apprenticeships.Web.Candidate;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof (StructuremapMvc), "Start")]

namespace SFA.Apprenticeships.Web.Candidate
{
    using Common.IoC;
    using Infrastructure.Address.IoC;
    using Infrastructure.Azure.Session.IoC;
    using Infrastructure.Caching.Azure.IoC;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.LocationLookup.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.RabbitMq.IoC;
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using Infrastructure.UserDirectory.IoC;
    using Infrastructure.VacancySearch.IoC;
    using IoC;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters.IoC;
    using StructureMap;

    /// <summary>
    ///     StructureMap MVC initialization. Sets the MVC resolver and the WebApi resolver to use structure map.
    /// </summary>
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var config = new ConfigurationManager();
            string useCacheSetting = config.TryGetAppSetting("UseCaching");
            bool useCache;
            bool.TryParse(useCacheSetting, out useCache);

#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<SessionRegistry>();

                // service layer
                x.AddRegistry<AzureCacheRegistry>();
                //x.AddRegistry<MemoryCacheRegistry>();

                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(useCache));
                x.AddRegistry<PostcodeRegistry>();
                // TODO: DEBT: AG: if Rabbit is incorrectly configured, website fails to start properly. Need to more lazily initialise RabbitMQ.
                x.AddRegistry<RabbitMqRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<AddressRegistry>();
                x.AddRegistry<PerformanceCounterRegistry>();

                // web layer
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<CandidateWebRegistry>();
            });

            WebCommonRegistry.Configure(ObjectFactory.Container);
#pragma warning restore 0618
        }
    }
}