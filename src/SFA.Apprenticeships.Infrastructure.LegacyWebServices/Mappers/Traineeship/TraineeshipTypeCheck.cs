namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Traineeship
{
    using AutoMapper;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;

    internal class TraineeshipTypeCheck : IMappingAction<GatewayServiceProxy.Vacancy, TraineeshipVacancyDetail>
    {
        public void Process(GatewayServiceProxy.Vacancy source, TraineeshipVacancyDetail destination)
        {
            if (source.VacancyType != "Traineeship")
            {
                throw new CustomException("Expected an traineeship, got a apprenticeship.", ErrorCodes.ApplicationTypeMismatch);
            }
        }
    }
}
