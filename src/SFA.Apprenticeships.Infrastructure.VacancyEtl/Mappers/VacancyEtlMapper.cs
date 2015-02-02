namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.Mappers
{
    using Application.VacancyEtl.Entities;
    using Common.Mappers;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;

    public class VacancyEtlMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipSummary, ApprenticeshipSummaryUpdate>();
            Mapper.CreateMap<TraineeshipSummary, TraineeshipSummaryUpdate>();
            Mapper.CreateMap<ApprenticeshipApplicationSummary, ExpiringDraft>();
        }
    }
}
