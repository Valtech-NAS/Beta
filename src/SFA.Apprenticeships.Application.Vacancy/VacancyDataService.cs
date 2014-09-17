namespace SFA.Apprenticeships.Application.Vacancy
{
    using System;
    using Domain.Entities.Vacancies;
    using Interfaces.Vacancies;
    using NLog;

    public class VacancyDataService : IVacancyDataService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyDataProvider _service;

        public VacancyDataService(IVacancyDataProvider service)
        {
            _service = service;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            Logger.Debug("Calling VacancyDataProvider to get vacancy details for user {0}.", vacancyId);

            try
            {
                return _service.GetVacancyDetails(vacancyId);
            }
            catch (Exception e)
            {
                var message = string.Format("Get vacancy failed for vacancy {0}.", vacancyId);
                Logger.DebugException(message, e);
                throw new Domain.Entities.Exceptions.CustomException(
                    message, e, ErrorCodes.GetVacancyDetailsFailed);
            }
        }
    }
}
