namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using Interfaces.Vacancies;
    using Domain.Entities.Vacancies;

    public class VacancyDataService : IVacancyDataService
    {
        private readonly IVacancyDataProvider _service;

        public VacancyDataService(IVacancyDataProvider service)
        {
            _service = service;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            return _service.GetVacancyDetails(vacancyId);
        }
    }
}
