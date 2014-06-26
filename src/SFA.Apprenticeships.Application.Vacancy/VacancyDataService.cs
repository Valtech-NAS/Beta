using SFA.Apprenticeships.Application.Interfaces.Vacancies;
using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;

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
