namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using Interfaces.Vacancy;
    using Domain.Entities.Vacancy;

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
