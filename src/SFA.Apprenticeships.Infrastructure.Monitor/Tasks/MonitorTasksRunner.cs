namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure;
    using NLog;
    using SFA.Apprenticeships.Infrastructure.PerformanceCounters;

    public class MonitorTasksRunner : IMonitorTasksRunner
    {
        private const string MonitorPerformanceCounterCategory = "SFA.Apprenticeships.WorkerRoles.Monitor";
        private const string MonitorCounter = "MonitorExecutions";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEnumerable<IMonitorTask> _monitorTasks;
        private readonly IPerformanceCounterService _performanceCounterService;

        public MonitorTasksRunner(IEnumerable<IMonitorTask> monitorTasks, 
            IPerformanceCounterService performanceCounterService)
        {
            _monitorTasks = monitorTasks;
            _performanceCounterService = performanceCounterService;
        }

        public void RunMonitorTasks()
        {
            Logger.Debug("Starting to run monitor tasks");

            var tasks = _monitorTasks.Select(mt => Task.Factory
                .StartNew(() =>
                {
                    Logger.Debug(string.Format("Start running task {0}", mt.TaskName));

                    try
                    {
                        mt.Run();
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(string.Format("Error while running task {0}", mt.TaskName), exception);
                    }

                    Logger.Debug(string.Format("Finished running task {0}", mt.TaskName));
                })).ToArray();

            Task.WaitAll(tasks);

            IncrementCounter();
            Logger.Debug("Finished running monitor tasks");
        }

        private void IncrementCounter()
        {
            bool performanceCountersEnabled;

            if (bool.TryParse(CloudConfigurationManager.GetSetting("PerformanceCountersEnabled"), out performanceCountersEnabled)
                && performanceCountersEnabled)
            {
                _performanceCounterService.IncrementCounter(MonitorPerformanceCounterCategory, MonitorCounter);
            }
        }
    }
}
