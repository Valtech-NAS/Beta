namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Application.Location;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Mappers;
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using StructureMap.Configuration.DSL;

    public class CandidateRegistry : Registry
    {
        public CandidateRegistry()
        {
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IMapper>().Singleton().Use<VacancySearchMapper>().Name = "VacancySearchMapper>";
            For<ISearchProvider>().Use<SearchProvider>().Ctor<IMapper>().Named("VacancySearchMapper>");
        }
    }
}
