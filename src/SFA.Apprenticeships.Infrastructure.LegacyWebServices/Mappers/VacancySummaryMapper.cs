namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Common.Mappers;
    using VacancySummaryProxy;

    public class VacancySummaryMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<VacancySummaryData, Domain.Entities.Vacancies.VacancySummary>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VacancyReference))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(d => d.VacancyLocationType, opt => opt.ResolveUsing<VacancyLocationTypeResolver>().FromMember(src => src.VacancyLocationType))
                .ForMember(d => d.Location, opt => opt.ResolveUsing<VacancySummaryLocationResolver>().FromMember(src => src.VacancyAddress))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.VacancyTitle));

            Mapper.CreateMap<VacancySummaryData[], IEnumerable<Domain.Entities.Vacancies.VacancySummary>>().ConvertUsing<SummaryDataConverter>();
        }
    }

    class SummaryDataConverter : ITypeConverter<VacancySummaryData[], IEnumerable<Domain.Entities.Vacancies.VacancySummary>>
    {
        public IEnumerable<Domain.Entities.Vacancies.VacancySummary> Convert(ResolutionContext context)
        {
            return
                from item in (VacancySummaryData[])context.SourceValue
                select context.Engine.Map<VacancySummaryData, Domain.Entities.Vacancies.VacancySummary>(item);
        }
    }
}