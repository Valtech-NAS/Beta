namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using SFA.Apprenticeships.Web.Candidate.Providers;
    using StructureMap.Configuration.DSL;

    public class CandidateRegistry : Registry
    {
        public CandidateRegistry()
        {
            For<ISearchProvider>().Use<SearchProvider>();
        }
    }
}
