namespace SFA.Apprenticeships.Domain.Interfaces.Services.Logging
{
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    public interface ILoggingService
    {
        void Trace(string message, params object[] parameters);
        void Debug(string message, params object[] parameters);
        void Info(string message, params object[] parameters);
        void Warn(string message, params object[] parameters);
        void Error(string message, params object[] parameters);
        void Fatal(string message, params object[] parameters);
    }
}
