namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using SFA.Apprenticeships.Application.Interfaces.Location;
    using SFA.Apprenticeships.Application.Location;
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using StructureMap.Configuration.DSL;

    public class CandidateRegistry : Registry
    {
        public CandidateRegistry()
        {
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ISearchProvider>().Use<SearchProvider>();
        }
    }
}
