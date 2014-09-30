namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Vacancy;
    using Application.VacancyEtl;
    using Domain.Entities.Vacancies;
    using NLog;

    public class CheckNasGateway : IMonitorTask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVacancyIndexDataProvider _vacancyIndexDataProvider;
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public CheckNasGateway(IVacancyIndexDataProvider vacancyIndexDataProvider, IVacancyDataProvider vacancyDataProvider)
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

                var vacancySummaries = summaries as IList<VacancySummary> ?? summaries.ToList();

                var summary = vacancySummaries.ToList().FirstOrDefault();

                if (summary != null)
                {
                    var vacancyDetail = _vacancyDataProvider.GetVacancyDetails(summary.Id);
                }
                else
                {
                    Logger.Error("Monitor get vacancy summary returned {0} records", vacancySummaries.Count());
                }
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Error connecting to NAS Gateway vacancy index", exception);
            }           
        }
    }
}