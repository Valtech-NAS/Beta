namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using Common.Mappers;
    using GatewayServiceProxy;

    public class ApplicationStatusSummaryMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<CandidateApplication, Domain.Entities.Applications.ApplicationStatusSummary>()
                .ForMember(d => d.LegacyApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(d => d.LegacyVacancyId, opt => opt.MapFrom(src => src.VacancyId))
                .ForMember(d => d.ApplicationStatus, opt => opt.ResolveUsing<ApplicationStatusResolver>().FromMember(src => src.ApplicationStatus))
                .ForMember(d => d.VacancyStatus, opt => opt.ResolveUsing<VacancyStatusResolver>().FromMember(src => src.VacancyStatus))
                .ForMember(d => d.ClosingDate, opt => opt.MapFrom(src => src.ClosingDate))
                .ForMember(d => d.UnsuccessfulReason, opt => opt.MapFrom(src => src.UnsuccessfulReason))
                .ForMember(d => d.WithdrawnOrDeclinedReason, opt => opt.MapFrom(src => src.WithdrawnOrDeclinedReason));
        }
    }
}
