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

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry() : this(false)
        {
        }

        public LegacyWebServicesRegistry(bool useCache)
        {
            For<IMapper>().Use<VacancySummaryMapper>().Name = "LegacyWebServices.VacancySummaryMapper";
            For<IMapper>().Use<VacancyDetailMapper>().Name = "LegacyWebServices.VacancyDetailMapper";
            For<ILegacyServicesConfiguration>().Singleton().Use(LegacyServicesConfiguration.Instance);
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IWcfService<IVacancyDetails>>().Use<WcfService<IVacancyDetails>>();
            For<IWcfService<GatewayServiceContract>>().Use<WcfService<GatewayServiceContract>>();

            For<IVacancyIndexDataProvider>()
                .Use<LegacyVacancyIndexDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.VacancySummaryMapper");

            #region Vacancy Data Service And Providers

            For<IVacancyDataProvider>()
                .Use<LegacyVacancyDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.VacancyDetailMapper")
                .Name = "LegacyVacancyDataProvider";

            if (useCache)
            {
                For<IVacancyDataProvider>()
                    .Use<CachedLegacyVacancyDataProvider>()
                    .Ctor<IVacancyDataProvider>()
                    .IsTheDefault()
                    .Ctor<IVacancyDataProvider>()
                    .Named("LegacyVacancyDataProvider");
            }

            For<IVacancyDataService>()
                .Use<VacancyDataService>()
                .Ctor<IVacancyDataProvider>();

            For<IVacancyStatusProvider>().Use<LegacyVacancyStatusProvider>();

            #endregion

            #region Candidate and Application Providers

            For<ILegacyCandidateProvider>().Use<LegacyCandidateProvider>();

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
