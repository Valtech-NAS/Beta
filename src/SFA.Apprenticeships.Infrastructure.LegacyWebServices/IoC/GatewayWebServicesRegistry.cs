namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using Application.ApplicationUpdate;
    using Application.Candidate.Strategies;
    using Application.Interfaces.Vacancies;
    using Application.Vacancy;
    using Application.VacancyEtl;
    using Configuration;
    using CreateApplication;
    using CreateCandidate;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using GetCandidateApplicationStatuses;
    using Mappers;
    using StructureMap.Configuration.DSL;
    using VacancyDetail;
    using VacancyDetailProxy;
    using VacancySummary;
    using VacancySummaryProxy;
    using Wcf;

    public class GatewayWebServicesRegistry : Registry
    {
        public GatewayWebServicesRegistry(): this(false)
        {
            
        }

        public GatewayWebServicesRegistry(bool useCache)
        {
            For<IMapper>().Use<GatewayVacancySummaryMapper>().Name = "LegacyWebServices.GatewayVacancySummaryMapper";
            For<IMapper>().Use<GatewayVacancyDetailMapper>().Name = "LegacyWebServices.GatewayVacancyDetailMapper";
            For<ILegacyServicesConfiguration>().Singleton().Use(LegacyServicesConfiguration.Instance);
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IWcfService<IVacancyDetails>>().Use<WcfService<IVacancyDetails>>();
            For<IWcfService<GatewayServiceContract>>().Use<WcfService<GatewayServiceContract>>();

            For<IVacancyIndexDataProvider>()
                .Use<GatewayVacancyIndexDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.GatewayVacancySummaryMapper");

            #region Vacancy Data Service And Providers

            For<IVacancyDataProvider>()
                .Use<GatewayVacancyDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.GatewayVacancyDetailMapper")
                .Name = "GatewayVacancyDataProvider";

            if (useCache)
            {
                For<IVacancyDataProvider>()
                    .Use<CachedLegacyVacancyDataProvider>()
                    .Ctor<IVacancyDataProvider>()
                    .IsTheDefault()
                    .Ctor<IVacancyDataProvider>()
                    .Named("GatewayVacancyDataProvider");
            }

            For<IVacancyDataService>()
                .Use<VacancyDataService>()
                .Ctor<IVacancyDataProvider>();

            For<IVacancyStatusProvider>().Use<LegacyVacancyStatusProvider>();

            #endregion

            #region Candidate and Application Providers

            For<ILegacyCandidateProvider>().Use<GatewayCandidateProvider>();

            For<ILegacyApplicationProvider>().Use<LegacyApplicationProvider>();

            For<IMapper>().Use<ApplicationStatusSummaryMapper>()
                .Name = "LegacyWebServices.ApplicationStatusSummaryMapper";

            For<ILegacyApplicationStatusesProvider>()
                .Use<LegacyCandidateApplicationStatusesProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.ApplicationStatusSummaryMapper")
                .Name = "LegacyCandidateApplicationStatusesProvider";

            #endregion
        }
    }
}