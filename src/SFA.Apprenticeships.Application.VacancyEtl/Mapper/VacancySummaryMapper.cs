namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using SFA.Apprenticeships.Application.Common.Mappers;
    using SFA.Apprenticeships.Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public class VacancySummaryMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<VacancySummary, VacancySummaryUpdate>();
            Mapper.CreateMap<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>().ConvertUsing<EnumerableVacancySummaryConverter>();
        }
    }

    class EnumerableVacancySummaryConverter : ITypeConverter<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>
    {
        public IEnumerable<VacancySummaryUpdate> Convert(ResolutionContext context)
        {
            return
                from item in (IEnumerable<VacancySummary>)context.SourceValue
                select context.Engine.Map<VacancySummary, VacancySummaryUpdate>(item);
        }
    }
}