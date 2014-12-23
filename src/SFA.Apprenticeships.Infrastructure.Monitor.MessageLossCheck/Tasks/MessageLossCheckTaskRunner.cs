namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Monitor.Tasks;
    using NLog;

    public class MessageLossCheckTaskRunner : IMessageLossCheckTaskRunner
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEnumerable<IMonitorTask> _monitorTasks;

        public MessageLossCheckTaskRunner(IEnumerable<IMonitorTask> monitorTasks)
        {
            _monitorTasks = monitorTasks;
        }

        public void RunMonitorTasks()
        {
            foreach (var monitorTask in _monitorTasks)
            {
                Logger.Info("Running " + monitorTask.TaskName);
                monitorTask.Run();
                if (monitorTask.GetType() == typeof(CheckUnsentCandidateMessages))
                {
                    Logger.Info("Waiting for 5 minutes to allow any application queue messages to be processed after fixing the associated candidate");
                    Thread.Sleep(TimeSpan.FromMinutes(5));
                }
            }
        }
    }
}