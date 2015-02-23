namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Domain.Entities;

    public class VoidEmailDispatcher : IEmailDispatcher
    {
        public void SendEmail(EmailRequest request)
        {
            //Does nothing as of now. 
        }
    }
}