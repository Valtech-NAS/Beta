namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Common.Mappers;
    using VacancySummaryProxy;

    public class VacancySummaryMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancySummaryData, Domain.Entities.Vacancies.VacancySummary>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VacancyReference))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(d => d.VacancyLocationType, opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))
                .ForMember(d => d.Location, opt => opt.ResolveUsing<VacancySummaryLocationResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle));
        }
    }
}