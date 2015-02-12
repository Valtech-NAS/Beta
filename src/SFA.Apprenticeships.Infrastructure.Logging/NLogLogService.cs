namespace SFA.Apprenticeships.Infrastructure.Logging
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Exceptions;
    using NLog;

    public class NLogLogService : ILogService
    {
        private readonly Logger _logger;

        public NLogLogService(Type type)
        {
            if (type == null)
            {
                _logger = LogManager.GetLogger(GetType().FullName);
                _logger.Warn("parentType was null. Using default for logger name");
            }
            else
            {
                _logger = LogManager.GetLogger(type.FullName);
            }
        }

        public void Debug(string message, params object[] args)
        {
            LogMessage(LogLevel.Debug, null, message, args);
        }

        public void Info(string message, params object[] args)
        {
            LogMessage(LogLevel.Info, null, message, args);
        }

        public void Warn(string message, Exception exception, params object[] args)
        {
            LogMessage(LogLevel.Warn, exception, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            LogMessage(LogLevel.Warn, null, message, args);
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            LogMessage(LogLevel.Error, exception, message, args);
        }

        public void Error(string message, params object[] args)
        {
            LogMessage(LogLevel.Error, null, message, args);
        }

        #region Helpers
        private void LogMessage(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            var logMessage = string.Format(message, args);
            var logEvent = new LogEventInfo
            {
                LoggerName = _logger.Name,
                Level = logLevel,
                Exception = exception,
                Message = logMessage
            };

            if (exception is CustomException)
            {
                logEvent.Properties["ErrorCode"] = (exception as CustomException).Code;
            }

            _logger.Log(logEvent);
        }
        #endregion
    }
}
