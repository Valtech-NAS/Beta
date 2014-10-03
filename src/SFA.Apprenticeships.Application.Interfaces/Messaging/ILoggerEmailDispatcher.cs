namespace SFA.Apprenticeships.Application.Interfaces.Messaging
{
    public interface ILoggerEmailDispatcher
    {
        void SendLogViaEmail(LogRequest request);
    }
}