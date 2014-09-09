namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using NLog;

    public class MonitorTasksRunner : IMonitorTasksRunner
    {
        private readonly IEnumerable<IMonitorTask> _monitorTasks;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MonitorTasksRunner(IEnumerable<IMonitorTask> monitorTasks)
        {
            _monitorTasks = monitorTasks;
        }

        public void RunMonitorTasks()
        {
            Logger.Debug("Starting to run {0} monitor tasks", _monitorTasks.Count());

            //todo: run tasks in parallel

            Logger.Debug("Finished running monitor tasks");

            throw new NotImplementedException();
        }
    }
}
