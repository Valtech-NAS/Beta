namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Interfaces.Logging;

    public class MonitorTasksRunner : IMonitorTasksRunner
    {
        private readonly ILogService _logger;
        private readonly IEnumerable<IMonitorTask> _monitorTasks;

        public MonitorTasksRunner(IEnumerable<IMonitorTask> monitorTasks, ILogService logger)
        {
            _monitorTasks = monitorTasks;
            _logger = logger;
        }

        public void RunMonitorTasks()
        {
            _logger.Info("Starting to run monitor tasks");

            var tasks = _monitorTasks.Select(mt => Task.Factory
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

            _logger.Debug("Finished running monitor tasks");
        }
    }
}
