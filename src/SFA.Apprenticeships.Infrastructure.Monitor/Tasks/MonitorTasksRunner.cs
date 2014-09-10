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

            var tasks = _monitorTasks.Select(mt => Task.Factory.StartNew(mt.Run)).ToArray();

            Task.WaitAll(tasks);

            Logger.Debug("Finished running monitor tasks");
        }
    }
}