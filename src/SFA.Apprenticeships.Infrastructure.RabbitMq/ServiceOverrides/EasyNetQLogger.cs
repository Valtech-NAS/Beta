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
            Logger.Debug(format, args);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            const string exceptionSubscriptionCallbackMessage = "Exception thrown by subscription callback";
            if (format.ToLowerInvariant().StartsWith(exceptionSubscriptionCallbackMessage.ToLowerInvariant()))
            {
                Logger.Error(exceptionSubscriptionCallbackMessage, new Exception(format));
            }
            else
            {
                Logger.Error(format, args);
            }
        }

        public void ErrorWrite(Exception exception)
        {
            Logger.Error("RabbitMQ Error Exception", exception);
        }
    }
}
