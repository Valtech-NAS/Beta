namespace SFA.Apprenticeships.Infrastructure.VacancyIndexer.Mappers
{
    using Application.VacancyEtl.Entities;
    using Common.Mappers;
    using Elastic.Common.Entities;

    public class VacancyIndexerMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointDomainToElasticResolver>().FromMember(src => src.Location));

            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSummaryUpdate>();

            Mapper.CreateMap<TraineeshipSummaryUpdate, TraineeshipSummary>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointDomainToElasticResolver>().FromMember(src => src.Location));

            Mapper.CreateMap<ApprenticeshipSummaryUpdate, ApprenticeshipSummary>();
        }
    }
}