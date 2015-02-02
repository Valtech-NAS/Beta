namespace SFA.Apprenticeships.Infrastructure.Monitor.MessageLossCheck.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Application.Interfaces.Logging;
    using Monitor.Tasks;

    public class MessageLossCheckTaskRunner : IMessageLossCheckTaskRunner
    {
        private readonly ILogService _logger;
        private readonly IEnumerable<IMonitorTask> _monitorTasks;

        public MessageLossCheckTaskRunner(IEnumerable<IMonitorTask> monitorTasks, ILogService logger)
        {
            _monitorTasks = monitorTasks;
            _logger = logger;
        }

        public void RunMonitorTasks()
        {
            foreach (var monitorTask in _monitorTasks)
            {
                _logger.Info("Running " + monitorTask.TaskName);
                monitorTask.Run();
 
                var checkUnsentCandidateMessages = monitorTask as CheckUnsentCandidateMessages;
                if (checkUnsentCandidateMessages != null && checkUnsentCandidateMessages.ActionsTaken)
                {
                    _logger.Info("Waiting for 5 minutes to allow any application queue messages to be processed after fixing the associated candidate");
                    Thread.Sleep(TimeSpan.FromMinutes(5));
                }
            }
        }
    }
}