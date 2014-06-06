namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.Common.Mapper;
    using SFA.Apprenticeships.Infrastructure.Elastic.Common.Entities;

    public class VacancyIndexerMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<VacancySummaryUpdate, VacancySummary>()
                .ForMember(d => d.Location, opt => opt.ResolveUsing<GeoPointDomainToElasticResolver>().FromMember(src => src.Location));

            Mapper.CreateMap<Domain.Entities.Vacancy.VacancySummary, VacancySummaryUpdate>();
            Mapper.CreateMap<IEnumerable<VacancySummaryUpdate>, IEnumerable<VacancySummary>>().ConvertUsing<EnumerableVacancySummaryConverter>();
            Mapper.CreateMap<IEnumerable<Domain.Entities.Vacancy.VacancySummary>, IEnumerable<VacancySummaryUpdate>>().ConvertUsing<EnumerableVacancySummaryUpdateConverter>();
        }
    }

    class EnumerableVacancySummaryConverter : ITypeConverter<IEnumerable<VacancySummaryUpdate>, IEnumerable<VacancySummary>>
    {
        public IEnumerable<VacancySummary> Convert(ResolutionContext context)
        {
            return
                from item in (IEnumerable<VacancySummaryUpdate>)context.SourceValue
                select context.Engine.Map<VacancySummaryUpdate, VacancySummary>(item);
        }
    }

    class EnumerableVacancySummaryUpdateConverter : ITypeConverter<IEnumerable<Domain.Entities.Vacancy.VacancySummary>, IEnumerable<VacancySummaryUpdate>>
    {
        public IEnumerable<VacancySummaryUpdate> Convert(ResolutionContext context)
        {
            return
                from item in (IEnumerable<Domain.Entities.Vacancy.VacancySummary>)context.SourceValue
                select context.Engine.Map<Domain.Entities.Vacancy.VacancySummary, VacancySummaryUpdate>(item);
        }
    }
}
