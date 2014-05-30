namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Application.Common.Mappers;
    using Application.VacancyEtl.Entities;
    using Elasticsearch.Entities;

    public class VacancySummaryMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<VacancySummaryUpdate, VacancySummary>();
            Mapper.CreateMap<IEnumerable<VacancySummaryUpdate>, IEnumerable<VacancySummary>>().ConvertUsing<EnumerableVacancySummaryConverter>();
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
}
