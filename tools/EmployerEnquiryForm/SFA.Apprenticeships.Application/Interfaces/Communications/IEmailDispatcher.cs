namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using Domain.Entities;

    public interface IEmailDispatcher
    {
        void SendEmail(EmailRequest request); 
    }
 
}