namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using NLog;

    public class MonitorTasksRunner : IMonitorTasksRunner
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEnumerable<IMonitorTask> _monitorTasks;

        public MonitorTasksRunner(IEnumerable<IMonitorTask> monitorTasks)
        {
            _monitorTasks = monitorTasks;
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
                        Logger.ErrorException(string.Format("Error while running task {0}", mt.TaskName), exception);
                    }

                    Logger.Debug(string.Format("Finished running task {0}", mt.TaskName));
                })).ToArray();

            Task.WaitAll(tasks);

            Logger.Debug("Finished running monitor tasks");
        }
    }
}
