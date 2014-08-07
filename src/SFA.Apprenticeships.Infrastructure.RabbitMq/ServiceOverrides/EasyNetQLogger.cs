namespace SFA.Apprenticeships.Infrastructure.RabbitMq.ServiceOverrides
{
    using System;
    using EasyNetQ;
    using NLog;

    public class EasyNetQLogger : IEasyNetQLogger
    {
        private static readonly Logger Logger = LogManager.GetLogger("RabbitMQ");
        
        public void DebugWrite(string format, params object[] args)
        {
            Logger.Debug(format, args);
        }

        public void InfoWrite(string format, params object[] args)
        {
            Logger.Info(format, args);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            Logger.Error(format, args);
        }

        public void ErrorWrite(Exception exception)
        {
            Logger.Error(exception);
        }
    }
}
