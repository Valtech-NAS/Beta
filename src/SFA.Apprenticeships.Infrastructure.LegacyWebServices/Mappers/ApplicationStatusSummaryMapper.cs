namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using Application.ApplicationUpdate.Entities;
    using Apprenticeships;
    using Common.Mappers;
    using GatewayServiceProxy;

    public class ApplicationStatusSummaryMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<CandidateApplication, ApplicationStatusSummary>()
                .ForMember(x => x.ApplicationId, y => y.Ignore())
                .ForMember(x => x.LegacyApplicationId, opt => opt.MapFrom(src => src.ApplicationId))
                .ForMember(x => x.LegacyVacancyId, opt => opt.MapFrom(src => src.VacancyId))
                .ForMember(x => x.ApplicationStatus, opt => opt.ResolveUsing<ApplicationStatusResolver>().FromMember(src => src.ApplicationStatus))
                .ForMember(x => x.VacancyStatus, opt => opt.ResolveUsing<VacancyStatusResolver>().FromMember(src => src.VacancyStatus))
                .ForMember(x => x.ClosingDate, opt => opt.MapFrom(src => src.ClosingDate))
                .ForMember(x => x.UnsuccessfulReason, opt => opt.MapFrom(src => src.UnsuccessfulReason));
        }
    }
}
