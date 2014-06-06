namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Application.VacancyEtl.Entities;
    using SFA.Apprenticeships.Infrastructure.Common.Mapper;

    public class VacancyEtlMapper : MapperEngine
    {
        public override void Initialize()
        {
            Mapper.CreateMap<Domain.Entities.Vacancy.VacancySummary, VacancySummaryUpdate>();
            Mapper.CreateMap<IEnumerable<Domain.Entities.Vacancy.VacancySummary>, IEnumerable<VacancySummaryUpdate>>().ConvertUsing<EnumerableVacancySummaryUpdateConverter>();
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
