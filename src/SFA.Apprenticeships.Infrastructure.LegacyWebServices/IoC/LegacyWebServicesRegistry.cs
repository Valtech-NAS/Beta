namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using SFA.Apprenticeships.Application.Common.Mappers;
    using SFA.Apprenticeships.Application.Interfaces.ReferenceData;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Application.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Common.Wcf;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceDataProxy;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummaryProxy;
    using SFA.Apprenticeships.Services.ReferenceData.Service;
    using StructureMap.Configuration.DSL;
    using ReferenceDataService = SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceData.ReferenceDataService;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry()
        {
            For<IMapper>().Use<VacancySummaryMapper>().Name = "LegacyWebServices.VacancySummaryMapper";
            For<ILegacyServicesConfiguration>().Singleton().Use(LegacyServicesConfiguration.Instance);
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();
            For<IVacancyProvider>().Use<VacancyProvider>().Ctor<IMapper>().Named("LegacyWebServices.VacancySummaryMapper");
            For<IVacancyService>().Use<VacancyService>();
            For<IReferenceDataService>().Use<ReferenceDataService>().Name = "Base.ReferenceDataService";
            For<IReferenceDataService>().Use<CachedReferenceDataService>().Ctor<IReferenceDataService>().Named("Base.ReferenceDataService");
        }
    }
}
