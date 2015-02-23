namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;

    public class DailyMetricsTasksRunner : IDailyMetricsTasksRunner
    {
        private readonly ILogService _logger;
        private readonly IEnumerable<IDailyMetricsTask> _tasks;

        public DailyMetricsTasksRunner(IEnumerable<IDailyMetricsTask> tasks, ILogService logger)
        {
            _tasks = tasks;
            _logger = logger;
        }

        public void RunDailyMetricsTasks()
        {
            _logger.Info("Starting to run daily metrics tasks");

            var tasks = _tasks.Select(mt => Task.Factory
                .StartNew(() =>
                {
                    _logger.Debug(string.Format("Start running task {0}", mt.TaskName));

                    try
                    {
                        mt.Run();
                    }
                    catch (Exception exception)
                    {
                        _logger.Error(string.Format("Error while running task {0}", mt.TaskName), exception);
                    }

                    _logger.Debug(string.Format("Finished running task {0}", mt.TaskName));
                })).ToArray();

            Task.WaitAll(tasks);

            _logger.Debug("Finished running daily metrics tasks");
        }
    }
}
