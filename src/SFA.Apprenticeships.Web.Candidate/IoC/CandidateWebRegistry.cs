namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using System;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Vacancies;
    using Application.Location;
    using Application.Vacancy;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using Providers;
    using StructureMap.Configuration.DSL;
    using Validators;
    using ViewModels.VacancySearch;

    public class CandidateWebRegistry : Registry
    {
        public CandidateWebRegistry()
        {
            // services (app)
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IVacancySearchService>().Use<VacancySearchService>();
            For<IVacancyDataService>().Use<VacancyDataService>();

            // providers (web)
            For<IMapper>().Singleton().Use<CandidateWebMappers>().Name = "CandidateWebMappers";
            For<ISearchProvider>().Use<SearchProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IVacancyDetailProvider>().Use<VacancyDetailProvider>().Ctor<IMapper>().Named("CandidateWebMappers");
            For<IApplicationProvider>().Use<ApplicationProvider>();

        }
    }
}
