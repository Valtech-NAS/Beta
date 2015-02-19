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

        public void Warn(Exception e, object data)
        {
            e.AddData(data);
            LogMessage(LogLevel.Warn, e, null, null);
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            LogMessage(LogLevel.Error, exception, message, args);
        }

        public void Error(string message, params object[] args)
        {
            LogMessage(LogLevel.Error, null, message, args);
        }

        public void Error(Exception e, object data)
        {
            e.AddData(data);
            LogMessage(LogLevel.Error, e);
        }

        #region Helpers

        private void LogMessage(LogLevel logLevel, Exception e, string message = null, params object[] args)
        {
            var logMessage = message == null ? e.Message : string.Format(message, args);

            var logEvent = new LogEventInfo
            {
                LoggerName = _logger.Name,
                Level = logLevel,
                Exception = e,
                Message = logMessage
            };

            if (e is CustomException)
            {
                logEvent.Properties["ErrorCode"] = (e as CustomException).Code;
                logEvent.Properties["Date"] = DateTime.UtcNow;
            }

            _logger.Log(logEvent);
        }

        #endregion
    }
}
