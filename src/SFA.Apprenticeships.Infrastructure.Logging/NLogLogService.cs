namespace SFA.Apprenticeships.Infrastructure.Logging
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Application.Interfaces.Logging;
    using NLog;

    public class NLogLogService : ILogService
    {
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
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Logger GetCallingLogger()
        {
            var fullClassName = GetFullClassName();
            var logger = LogManager.GetLogger(fullClassName);

            return logger;
        }

        private static string GetFullClassName()
        {
            string className;
            Type declaringType;
            var framesToSkip = 4;

            do
            {
                var frame = new StackFrame(framesToSkip, false);
                var method = frame.GetMethod();
                declaringType = method.DeclaringType;

                if (declaringType == null)
                {
                    className = method.Name;
                    break;
                }

                framesToSkip++;
                className = declaringType.FullName;
            } while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return className;
        }

        private static void LogMessage(LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            var logger = GetCallingLogger();
            var logMessage = string.Format(message, args);

            logger.Log(logLevel, logMessage, exception);
        }

        #endregion
    }
}
