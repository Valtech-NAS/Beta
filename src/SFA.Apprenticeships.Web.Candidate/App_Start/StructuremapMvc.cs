using SFA.Apprenticeships.Web.Candidate;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]

namespace SFA.Apprenticeships.Web.Candidate
{
    using Infrastructure.Azure.Session.IoC;
    using Infrastructure.Common.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.LocationLookup.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.VacancySearch.IoC;
    using IoC;
    using Common.IoC;
    using StructureMap;

    /// <summary>
    /// StructureMap MVC initialization. Sets the MVC resolver and the WebApi resolver to use structure map.
    /// </summary>
    public static class StructuremapMvc
    {
        public static void Start()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();

                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<LocationLookupRegistry>();

                x.AddRegistry<SessionRegistry>();
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<CandidateRegistry>();
            });

            WebCommonRegistry.Configure(ObjectFactory.Container);
        }
    }
}