
namespace SFA.Apprenticeships.Services.Legacy.Vacancy.IoC
{
    using SFA.Apprenticeships.Common.Interfaces.Mapper;
    using SFA.Apprenticeships.Common.Interfaces.Services;
    using SFA.Apprenticeships.Services.Common.Wcf;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Abstract;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Mappers;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Proxy;
    using SFA.Apprenticeships.Services.Legacy.Vacancy.Service;
    using StructureMap.Configuration.DSL;

    public class VacancySummaryRegistry : Registry
    {
        public VacancySummaryRegistry()
        {
            For<IWcfService<IVacancySummary>>().Use<WcfService<IVacancySummary>>();
            For<IMapper>().Singleton().Use<VacancySummaryMapper>().Named("VacancySummaryMapper").OnCreation(x => x.Initialize());
            For<IVacancySummaryService>().Use<VacancySummaryService>().Ctor<IMapper>().Named("VacancySummaryMapper");
        }
    }
}
