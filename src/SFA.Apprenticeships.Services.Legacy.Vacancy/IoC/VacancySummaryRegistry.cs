namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IoC
{
    using SFA.Apprenticeships.Common.Interfaces.Services;
    using SFA.Apprenticeships.Services.Common.Wcf;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;
    using StructureMap.Configuration.DSL;

    public class VacancySummaryRegistry : Registry
    {
        public VacancySummaryRegistry()
        {
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IVacancySummaryService>().Use<VacancySummaryService>();
        }
    }
}
