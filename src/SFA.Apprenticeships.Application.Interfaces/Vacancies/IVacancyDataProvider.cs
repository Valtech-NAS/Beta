using System;
using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Application.Interfaces.Vacancies
{
    public interface IVacancyDataProvider
    {
        VacancyDetail GetVacancyDetails(int vacancyId);
    }
}
