namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using Common.Mappers;

    public class LegacyVacancySummaryMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<GatewayServiceProxy.VacancySummary, Domain.Entities.Vacancies.VacancySummary>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.VacancyId))

                .ForMember(dest => dest.ClosingDate,
                    opt => opt.MapFrom(src => src.ClosingDate))

                .ForMember(dest => dest.EmployerName,
                    opt => opt.MapFrom(src => src.EmployerName))

                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.ShortDescription))

                .ForMember(dest => dest.Location,
                    opt => opt.ResolveUsing<LegacyVacancySummaryLocationResolver>()
                        .FromMember(src => src.Address))

                .ForMember(dest => dest.VacancyLocationType,
                    opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))

                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.VacancyTitle))

                .ForMember(dest => dest.VacancyType,
                    opt => opt.ResolveUsing<VacancyTypeResolver>().FromMember(src => src.VacancyType));
        }
    }
}
