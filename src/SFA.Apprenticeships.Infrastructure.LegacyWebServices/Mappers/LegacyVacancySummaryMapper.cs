namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using Apprenticeships;
    using Common.Mappers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using GatewayServiceProxy;

    public class LegacyVacancySummaryMapper : MapperEngine
    {
        public override void Initialise()
        {
            CreateApprenticeshipSummaryMap();
            CreateTraineeshipSummaryMap();
        }

        private void CreateApprenticeshipSummaryMap()
        {
            Mapper.CreateMap<VacancySummary, ApprenticeshipSummary>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.VacancyId))

                .ForMember(dest => dest.VacancyReference,
                    opt => opt.MapFrom(src => src.VacancyReference))

                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.PossibleStartDate))

                .ForMember(dest => dest.ClosingDate,
                    opt => opt.MapFrom(src => src.ClosingDate))

                .ForMember(dest => dest.EmployerName,
                    opt => opt.MapFrom(src => src.EmployerName))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.ShortDescription))

                .ForMember(dest => dest.Location,
                    opt => opt.ResolveUsing<LegacyVacancySummaryLocationResolver>()
                        .FromMember(src => src.Address))

                .ForMember(dest => dest.ApprenticeshipLevel,
                    opt => opt.ResolveUsing<SummaryApprenticeshipLevelResolver>().FromMember(src => src))

                .ForMember(dest => dest.VacancyLocationType,
                    opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.VacancyTitle))

                .ForMember(dest => dest.Sector,
                    opt => opt.MapFrom(src => src.ApprenticeshipOccupation))

                .ForMember(dest => dest.Framework,
                    opt => opt.MapFrom(src => src.ApprenticeshipFrameworkDescription))

                .ForMember(dest => dest.SectorCode, opt => opt.Ignore())

                .ForMember(dest => dest.FrameworkCode, opt => opt.Ignore());


        }

        private void CreateTraineeshipSummaryMap()
        {
            Mapper.CreateMap<VacancySummary, TraineeshipSummary>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.VacancyId))

                .ForMember(dest => dest.VacancyReference,
                    opt => opt.MapFrom(src => src.VacancyReference))

                .ForMember(dest => dest.StartDate,
                    opt => opt.MapFrom(src => src.PossibleStartDate))

                .ForMember(dest => dest.ClosingDate,
                    opt => opt.MapFrom(src => src.ClosingDate))

                .ForMember(dest => dest.EmployerName,
                    opt => opt.MapFrom(src => src.EmployerName))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.ShortDescription))

                .ForMember(dest => dest.Location,
                    opt => opt.ResolveUsing<LegacyVacancySummaryLocationResolver>()
                        .FromMember(src => src.Address))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.VacancyTitle))

                .ForMember(dest => dest.Sector,
                    opt => opt.MapFrom(src => src.ApprenticeshipOccupation))

                .ForMember(dest => dest.Framework,
                    opt => opt.MapFrom(src => src.ApprenticeshipFrameworkDescription))

                .ForMember(dest => dest.SectorCode, opt => opt.Ignore())

                .ForMember(dest => dest.FrameworkCode, opt => opt.Ignore());
        }
    }
}
