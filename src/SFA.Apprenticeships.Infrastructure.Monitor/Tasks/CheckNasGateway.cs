using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;

namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Vacancies.Traineeships;
    using NLog;
    using SFA.Apprenticeships.Application.Vacancy;
    using SFA.Apprenticeships.Application.VacancyEtl;
    using SFA.Apprenticeships.Domain.Entities.Vacancies;

    public class CheckNasGateway : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyDataProvider _vacancyDataProvider;
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;

        public CheckNasGateway(IVacancyIndexDataProvider vacancyIndexDataProvider,
            IVacancyDataProvider vacancyDataProvider)
        {
            _vacancyIndexDataProvider = vacancyIndexDataProvider;
            _vacancyDataProvider = vacancyDataProvider;
        }

        public string TaskName
        {
            get { return "Check NAS gateway"; }
        }

        public void Run()
        {
            try
            {
                var summaries = _vacancyIndexDataProvider.GetVacancySummaries(1);

                var apprenticeshipSummaries = summaries as IList<ApprenticeshipSummary> ?? summaries.ApprenticeshipSummaries;
                var traineeshipSummaries = summaries as IList<TraineeshipSummary> ?? summaries.TraineeshipSummaries;

                var summary = apprenticeshipSummaries.ToList().FirstOrDefault();

                if (summary != null)
                {
                    var apprenticeshipDetail = _vacancyDataProvider.GetVacancyDetails(summary.Id);
                }
                else
                {
                    Logger.Error("Monitor get vacancy summary returned {0} records", apprenticeshipSummaries.Count() + traineeshipSummaries.Count());
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Error connecting to NAS Gateway vacancy index", exception);
            }
        }
    }
}