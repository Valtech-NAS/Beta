namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IoC
{
    using StructureMap.Configuration.DSL;

    public class VacancySummaryRegistry : Registry
    {
        public VacancySummaryRegistry()
        {
            //For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            //For<IMapper>().Singleton().Use<VacancySummaryMapper>().Named("VacancySummaryMapper").OnCreation(x => x.Initialize());
            //For<IVacancySummaryService>().Use<VacancySummaryService>().Ctor<IMapper>().Named("VacancySummaryMapper");
        }
    }
}
