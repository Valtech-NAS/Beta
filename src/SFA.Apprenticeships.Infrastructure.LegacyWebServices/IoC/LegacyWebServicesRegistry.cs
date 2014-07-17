namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using Application.Candidate.Strategies;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.ReferenceData;
    using Application.VacancyEtl;
    using Configuration;
    using CreateCandidate;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using Mappers;
    using ReferenceData;
    using ReferenceDataProxy;
    using StructureMap.Configuration.DSL;
    using VacancyDetail;
    using VacancyDetailProxy;
    using VacancySummary;
    using VacancySummaryProxy;
    using Wcf;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry()
        {
            For<IMapper>().Use<VacancySummaryMapper>().Name = "LegacyWebServices.VacancySummaryMapper";
            For<IMapper>().Use<VacancyDetailMapper>().Name = "LegacyWebServices.VacancyDetailMapper";
            For<ILegacyServicesConfiguration>().Singleton().Use(LegacyServicesConfiguration.Instance);
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IWcfService<IVacancyDetails>>().Use<WcfService<IVacancyDetails>>();
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();
            For<IWcfService<GatewayServiceContract>>().Use<WcfService<GatewayServiceContract>>();
            For<IVacancyIndexDataProvider>()
                .Use<LegacyVacancyIndexDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.VacancySummaryMapper");
            For<IVacancyDataProvider>()
                .Use<LegacyVacancyDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.VacancyDetailMapper");

            For<IReferenceDataProvider>()
                .Use<LegacyReferenceDataProvider>()
                .Name = "LegacyReferenceDataProvider";

            For<IReferenceDataProvider>()
                .Use<CachedLegacyReferenceDataProvider>()
                .Ctor<IReferenceDataProvider>()
                .Named("LegacyReferenceDataProvider")
                .Name = "CachedLegacyReferenceDataProvider";

            For<IReferenceDataService>()
                .Use<ReferenceDataService>()
                .Ctor<IReferenceDataProvider>()
                .Named("CachedLegacyReferenceDataProvider");

            For<ILegacyCandidateProvider>().Use<LegacyCandidateProvider>();
        }
    }
}