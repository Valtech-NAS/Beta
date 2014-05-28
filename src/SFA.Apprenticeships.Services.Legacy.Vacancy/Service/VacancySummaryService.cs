namespace SFA.Apprenticeships.Services.Legacy.Vacancy.Service
{
    using System;
    using System.Collections.Generic;
    using SFA.Apprenticeships.Application.Interfaces.Vacancy;
    using SFA.Apprenticeships.Domain.Entities.Vacancy;

    public class VacancySummaryService : IVacancySummaryService
    {

        private readonly IVacancySummaryService _service;

        public VacancySummaryService(IVacancySummaryService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            _service = service;
        }

        public int GetVacancyPageCount(VacancyLocationType vacancyLocationType)
        {
            return _service.GetVacancyPageCount(vacancyLocationType);
        }

        public IEnumerable<VacancySummary> GetVacancySummary(VacancyLocationType vacancyLocationType, int page = 1)
        {
            return _service.GetVacancySummary(vacancyLocationType, page);
        }
    }
}
