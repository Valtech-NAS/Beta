namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using Application.VacancyEtl.Entities;
    using Common.Mappers;
    using Elastic.Common.Entities;

    public class VacancyIndexerMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancySummaryUpdate, VacancySummary>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointDomainToElasticResolver>().FromMember(src => src.Location));

            Mapper.CreateMap<Domain.Entities.Vacancies.VacancySummary, VacancySummaryUpdate>();
        }
    }
}