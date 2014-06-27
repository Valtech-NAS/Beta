namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using Application.Interfaces.Locations;
    using Application.Location;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using Providers;
    using StructureMap.Configuration.DSL;
    using Validators;
    using ViewModels.VacancySearch;

    public class CandidateRegistry : Registry
    {
        public CandidateRegistry()
        {
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IMapper>().Singleton().Use<CandidateWebMappers>().Name = "CandidateWebMappers";
            For<ISearchProvider>().Use<SearchProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IVacancyDetailProvider>().Use<VacancyDetailProvider>().Ctor<IMapper>().Named("CandidateWebMappers");

            For<IValidateModel<VacancySearchViewModel>>().Use<VacancySearchValidator>();
        }
    }
}