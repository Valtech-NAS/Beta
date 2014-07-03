namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mappers
{
    using Application.VacancyEtl.Entities;
    using Common.Mappers;
    using Domain.Entities.Vacancies;

    public class VacancyEtlMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancySummary, VacancySummaryUpdate>();
        }
    }
}