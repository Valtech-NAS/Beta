namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Mappers
{
    using System.Collections.Generic;
    using Application.Interfaces.Vacancies;
    using Common.Mappers;
    using Elastic.Common.Entities;

    public class VacancySearchMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSummaryResponse>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointElasticToDomainResolver>().FromMember(src => src.Location));

            Mapper.CreateMap<TraineeshipSummary, TraineeshipSummaryResponse>()
                .ForMember(d => d.Location,
                    opt => opt.ResolveUsing<GeoPointElasticToDomainResolver>().FromMember(src => src.Location));
        }
    }
}