namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using SFA.Apprenticeships.Application.Common.Mappers;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Infrastructure.Common.Wcf;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Configuration;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.ReferenceDataProxy;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary;
    using SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummaryProxy;
    using StructureMap.Configuration.DSL;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry()
        {
            For<IMapper>().Use<VacancySummaryMapper>().Name = "LegacyWebServices.VacancySummaryMapper";
            For<ILegacyServicesConfiguration>().Singleton().Use(LegacyServicesConfiguration.Instance);
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();
            For<IVacancySummaryService>().Use<VacancySummaryService>().Ctor<IMapper>().Named("LegacyWebServices.VacancySummaryMapper");
        }
    }
}
