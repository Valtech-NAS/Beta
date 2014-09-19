namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;

    public class CheckNasGateway : IMonitorTask
    {
        public string TaskName
        {
            get { return "Check NAS gateway"; }
        }

        public void Run()
        {
            //todo: invoke vacancy detail
    }
    }
}