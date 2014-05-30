namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using SFA.Apprenticeships.Application.Common.Mappers;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.Elasticsearch.Entities;

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