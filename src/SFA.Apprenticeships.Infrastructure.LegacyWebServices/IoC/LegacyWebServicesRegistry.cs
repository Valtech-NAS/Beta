namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancy;
    using Application.ReferenceData;
    using Application.Vacancy;
    using Common.Wcf;
    using Domain.Interfaces.Services.Mapping;
    using Configuration;
    using Mappers;
    using ReferenceDataProxy;
    using VacancySummary;
    using VacancySummaryProxy;
    using StructureMap.Configuration.DSL;
    using ReferenceData;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry()
        {
            For<IMapper>().Use<VacancySummaryMapper>().Name = "LegacyWebServices.VacancySummaryMapper";
            For<ILegacyServicesConfiguration>().Singleton().Use(LegacyServicesConfiguration.Instance);
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();
            For<IVacancyProvider>().Use<LegacyVacancyProvider>().Ctor<IMapper>().Named("LegacyWebServices.VacancySummaryMapper");
            For<IVacancyService>().Use<VacancyService>();
            For<IReferenceDataProvider>().Use<LegacyReferenceDataProvider>();
            For<IReferenceDataService>().Use<ReferenceDataService>().Name = "Base.ReferenceDataService";
            For<IReferenceDataService>().Use<CachedReferenceDataService>().Ctor<IReferenceDataService>().Named("Base.ReferenceDataService");
        }
    }
}
