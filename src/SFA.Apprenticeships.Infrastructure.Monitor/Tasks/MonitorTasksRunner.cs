namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using NLog;

    public class MonitorTasksRunner : IMonitorTasksRunner
    {
        private readonly IEnumerable<IMonitorTask> _monitorTasks;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MonitorTasksRunner()
        {
        }

        public void RunMonitorTasks()
        {
            Logger.Debug("Starting to run monitor tasks");

            //todo: run tasks in parallel

            Logger.Debug("Finished running monitor tasks");

            throw new NotImplementedException();
        }
    }
}
