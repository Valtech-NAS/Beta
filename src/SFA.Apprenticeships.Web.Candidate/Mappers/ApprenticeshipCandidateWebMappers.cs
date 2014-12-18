namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Account;
    using ViewModels.Applications;
    using ViewModels.Locations;
    using ViewModels.Register;
    using ViewModels.VacancySearch;

    public class ApprenticeshipCandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<SearchResults<ApprenticeshipSummaryResponse>, ApprenticeshipSearchResponseViewModel>()
                .ConvertUsing<ApprenticeshipSearchResultsResolver>();

            Mapper.CreateMap<ApprenticeshipSearchViewModel, Location>()
                .ConvertUsing<LocationResolver>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.GeoPoint.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.GeoPoint.Longitude));

            Mapper.CreateMap<ApprenticeshipVacancyDetail, VacancyDetailViewModel>()
                .ForMember(d => d.EmployerName,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.EmployerNameResolver>())
                .ForMember(d => d.Wage,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.WageResolver>())
                .ForMember(d => d.RealityCheck,
                    opt => opt.MapFrom(src => src.RealityCheck))
                .ForMember(d => d.OtherInformation,
                    opt => opt.MapFrom(src => src.OtherInformation))
                .ForMember(d => d.ApplyViaEmployerWebsite,
                    opt => opt.MapFrom(src => src.ApplyViaEmployerWebsite))
                .ForMember(d => d.VacancyUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.UrlResolver>()
                        .FromMember(src => src.VacancyUrl))
                .ForMember(d => d.IsWellFormedVacancyUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.IsWellFormedUrlResolver>()
                        .FromMember(src => src.VacancyUrl))
                .ForMember(d => d.ApplicationInstructions,
                    opt => opt.MapFrom(src => src.ApplicationInstructions))
                .ForMember(d => d.IsRecruitmentAgencyAnonymous,
                    opt => opt.MapFrom(src => src.IsRecruitmentAgencyAnonymous))
                .ForMember(d => d.RecruitmentAgency,
                    opt => opt.MapFrom(src => src.RecruitmentAgency))
                .ForMember(d => d.IsEmployerAnonymous,
                    opt => opt.MapFrom(src => src.IsEmployerAnonymous))
                .ForMember(d => d.IsNasProvider,
                    opt => opt.MapFrom(src => src.IsNasProvider))
                .ForMember(d => d.EmployerWebsite,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.UrlResolver>()
                        .FromMember(src => src.EmployerWebsite))
                .ForMember(d => d.IsWellFormedEmployerWebsiteUrl,
                    opt => opt.ResolveUsing<VacancyDetailViewModelResolvers.IsWellFormedUrlResolver>()
                        .FromMember(src => src.EmployerWebsite))
                .ForMember(d => d.VacancyType,
                        opt => opt.Ignore())
                .ForMember(d => d.CandidateApplicationStatus,
                        opt => opt.Ignore())
                .ForMember(d => d.DateApplied,
                        opt => opt.Ignore())
                .ForMember(d => d.ViewModelMessage,
                        opt => opt.Ignore())
                 .ForMember(d => d.HasCandidateAlreadyApplied,
                    opt=> opt.Ignore());

            Mapper.CreateMap<ApprenticeshipSummaryResponse, ApprenticeshipVacancySummaryViewModel>();
            
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<AddressViewModel, Address>();

            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<GeoPointViewModel, GeoPoint>();

            Mapper.CreateMap<RegisterViewModel, Candidate>()
                .ConvertUsing<CandidateResolver>();

            Mapper.CreateMap<ApprenticheshipApplicationViewModel, ApprenticeshipApplicationDetail>()
                .ConvertUsing<ApprenticeshipApplicationViewModelToApprenticeshipApplicationDetailResolver>();

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApprenticheshipApplicationViewModel>()
                .ConvertUsing<ApprenticeshipApplicationDetailToApprenticeshipApplicationViewModelResolver>();

            Mapper.CreateMap<RegistrationDetails, SettingsViewModel>()
                .ConvertUsing<SettingsViewModelResolvers.RegistrationDetailsToSettingsViewModelResolver>();
        }
    }
}