namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using SFA.Apprenticeships.Application.Interfaces.Search;
    using SFA.Apprenticeships.Application.Interfaces.Vacancies;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Domain.Entities.Candidates;
    using SFA.Apprenticeships.Domain.Entities.Locations;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    using SFA.Apprenticeships.Infrastructure.Common.Mappers;
    using SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Account;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class TraineeshipCandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            //------------
            Mapper.CreateMap<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>()
                .ConvertUsing<SearchResultsResolver>();

            Mapper.CreateMap<VacancySearchViewModel, Location>()
                .ConvertUsing<LocationResolver>();

            Mapper.CreateMap<Location, LocationViewModel>()
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.GeoPoint.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.GeoPoint.Longitude));

            //------------
            Mapper.CreateMap<VacancyDetail, VacancyDetailViewModel>()
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
                        opt => opt.Ignore());

            //------------

            Mapper.CreateMap<VacancySummaryResponse, VacancySummaryViewModel>();

            //----------

            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<AddressViewModel, Address>();

            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<GeoPointViewModel, GeoPoint>();

            //---------------
            Mapper.CreateMap<RegisterViewModel, Candidate>()
                .ConvertUsing<CandidateResolver>();
            //----------

            Mapper.CreateMap<TraineeshipApplicationViewModel, TraineeshipApplicationDetail>()
                .ConvertUsing<TraineeeshipApplicationViewModelToTraineeeshipApplicationDetailResolver>();

            Mapper.CreateMap<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>()
                .ConvertUsing<TraineeshipApplicationDetailToTraineeshipApplicationViewModelResolver>();

            //-----------
            Mapper.CreateMap<RegistrationDetails, SettingsViewModel>()
                .ConvertUsing<SettingsViewModelResolvers.RegistrationDetailsToSettingsViewModelResolver>();
            //----------

        }
    }
}