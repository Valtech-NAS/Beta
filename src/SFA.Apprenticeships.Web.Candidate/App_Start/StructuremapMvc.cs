using SFA.Apprenticeships.Infrastructure.Address.IoC;
using SFA.Apprenticeships.Web.Candidate;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StructuremapMvc), "Start")]

namespace SFA.Apprenticeships.Web.Candidate
{
    using Infrastructure.Repositories.Applications.IoC;
    using Infrastructure.Azure.Session.IoC;
    using Infrastructure.Repositories.Candidates.IoC;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.LocationLookup.IoC;
    using Infrastructure.Postcode.IoC;
    using Infrastructure.VacancySearch.IoC;
    using Infrastructure.Repositories.Users.IoC;
    using IoC;
    using Common.IoC;
    using StructureMap;
    using Infrastructure.Common.IoC;
    using Infrastructure.UserDirectory.IoC;

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

                // service layer
                x.AddRegistry<VacancySearchRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.AddRegistry<PostcodeRegistry>();
                x.AddRegistry<LocationLookupRegistry>();
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<UserRepositoryRegistry>();
                x.AddRegistry<UserDirectoryRegistry>();
                x.AddRegistry<AddressRegistry>();

                // web layer
                x.AddRegistry<SessionRegistry>();
                x.AddRegistry<WebCommonRegistry>();
                x.AddRegistry<CandidateWebRegistry>();
            });

            WebCommonRegistry.Configure(ObjectFactory.Container);
        }
    }
}