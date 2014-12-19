namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers.Apprenticeships
{
    using AutoMapper;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Vacancies.Apprenticeships;

    internal class ApprenticeshipTypeCheck : IMappingAction<GatewayServiceProxy.Vacancy, ApprenticeshipVacancyDetail>
    {
        public void Process(GatewayServiceProxy.Vacancy source, ApprenticeshipVacancyDetail destination)
        {
            if (source.VacancyType == "Traineeship")
            {
                throw new CustomException("Expected an apprenticeship, got a traineeship.", ErrorCodes.ApplicationTypeMismatch);
            }
        }
    }
}
