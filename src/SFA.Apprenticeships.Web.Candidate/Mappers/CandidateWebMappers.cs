namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Applications;
    using ViewModels.Locations;
    using ViewModels.Register;
    using ViewModels.VacancySearch;

    public class CandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>()
                .ConvertUsing<SearchResultsResolver>();

            Mapper.CreateMap<VacancySearchViewModel, Location>()
                .ConvertUsing<LocationResolver>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.GeoPoint.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.GeoPoint.Longitude));

            Mapper.CreateMap<VacancyDetail, VacancyDetailViewModel>();
            Mapper.CreateMap<VacancySummaryResponse, VacancySummaryViewModel>();
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();

            Mapper.CreateMap<RegisterViewModel, Candidate>()
                .ConvertUsing<CandidateResolver>();

            Mapper.CreateMap<ApplicationViewModel, ApplicationDetail>()
                .ConvertUsing<ApplicationViewModelToApplicationDetailResolver>();

            Mapper.CreateMap<ApplicationDetail, ApplicationViewModel>()
                .ConvertUsing<ApplicationDetailToApplicationViewModelResolver>();
        }
    }
}