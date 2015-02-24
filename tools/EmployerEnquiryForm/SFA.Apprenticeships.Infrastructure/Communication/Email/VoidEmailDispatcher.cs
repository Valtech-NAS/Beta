namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using Application.Interfaces.Communications;
    using Domain.Entities;
    using System;
    using System.Net;
    using System.Net.Mail;
    using SendGrid;

    public class VoidEmailDispatcher : IEmailDispatcher
    {
        public void SendEmail(EmailRequest request)
        {
            //Does nothing as of now. 
        }
    }
}