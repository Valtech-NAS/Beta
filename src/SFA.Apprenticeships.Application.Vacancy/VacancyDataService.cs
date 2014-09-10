namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using Domain.Entities.Vacancies;
    using Interfaces.Vacancies;

    public class VacancyDataService : IVacancyDataService
    {
        private readonly IVacancyDataProvider _service;

        public VacancyDataService(IVacancyDataProvider service)
        {
            _service = service;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            try
            {
                return _service.GetVacancyDetails(vacancyId);
            }
            catch (Exception e)
            {
                var message = string.Format("Get vacancy failed for vacancy {0}.", vacancyId);

                throw new Domain.Entities.Exceptions.CustomException(
                    message, e, ErrorCodes.GetVacancyDetailsFailed);
            }
        }
    }
}
