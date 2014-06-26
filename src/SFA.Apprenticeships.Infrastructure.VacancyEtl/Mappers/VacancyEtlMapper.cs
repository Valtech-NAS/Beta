using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Application.VacancyEtl.Entities;
    using Common.Mappers;

    public class VacancyEtlMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<VacancySummary, VacancySummaryUpdate>();
            Mapper.CreateMap<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>().ConvertUsing<EnumerableVacancySummaryUpdateConverter>();
        }
    }

    class EnumerableVacancySummaryUpdateConverter : ITypeConverter<IEnumerable<VacancySummary>, IEnumerable<VacancySummaryUpdate>>
    {
        public IEnumerable<VacancySummaryUpdate> Convert(ResolutionContext context)
        {
            return
                from item in (IEnumerable<VacancySummary>)context.SourceValue
                select context.Engine.Map<VacancySummary, VacancySummaryUpdate>(item);
        }
    }
}
