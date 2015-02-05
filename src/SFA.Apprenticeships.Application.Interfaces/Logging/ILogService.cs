namespace SFA.Apprenticeships.Application.Interfaces.Logging
{
    using System;

    //todo: add domain specific overloads? (entity, custom exception, etc.)
    public interface ILogService
    {
        void Debug(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warn(string message, Exception exception, params object[] args);

        void Warn(string message, params object[] args);

        void Error(string message, Exception exception, params object[] args);

        void Error(string message, params object[] args);
    }
}
