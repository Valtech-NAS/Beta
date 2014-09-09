namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Account;
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

            // TODO: US509 etc.: AG: REMOVE these mappers when new NAS Gateway service is available (replacement is GatewayVacancyDetailMapper.cs).
            Mapper.CreateMap<VacancyDetail, VacancyDetailViewModel>()
                .ForMember(d => d.EmployerName,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.EmployerNameResolver>())
                .ForMember(d => d.Wage, opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.WageResolver>())
                .ForMember(d => d.RealityCheck,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.LoremIpsumResolver>())
                .ForMember(d => d.OtherInformation,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.LoremIpsumResolver>())
                .ForMember(d => d.ApplyViaEmployerWebsite,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.ApplyViaEmployerWebsiteResolver>())
                .ForMember(d => d.VacancyUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.VacancyUrlesolver>())
                .ForMember(d => d.ApplicationInstructions,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.ApplicationInstructionsResolver>());

            Mapper.CreateMap<VacancySummaryResponse, VacancySummaryViewModel>();
            
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<AddressViewModel, Address>();

            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<GeoPointViewModel, GeoPoint>();

            Mapper.CreateMap<RegisterViewModel, Candidate>()
                .ConvertUsing<CandidateResolver>();

            Mapper.CreateMap<ApplicationViewModel, ApplicationDetail>()
                .ConvertUsing<ApplicationViewModelToApplicationDetailResolver>();

            Mapper.CreateMap<ApplicationDetail, ApplicationViewModel>()
                .ConvertUsing<ApplicationDetailToApplicationViewModelResolver>();

            Mapper.CreateMap<RegistrationDetails, SettingsViewModel>()
                .ConvertUsing<SettingsViewModelResolvers.RegistrationDetailsToSettingsViewModelResolver>();
        }
    }
}