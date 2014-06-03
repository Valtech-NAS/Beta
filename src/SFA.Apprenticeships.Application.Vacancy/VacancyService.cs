namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Interfaces.Vacancy;
    using Domain.Entities.Vacancy;

    public class VacancyService : IVacancyService
    {
        private readonly IVacancyProvider _service;

        public VacancyService(IVacancyProvider service)
        {
            Condition.Requires(service, "service").IsNotNull();

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
