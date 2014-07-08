namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using Application.Interfaces.Messaging;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        public void SendEmail(EmailRequest request)
        {
            //todo: SendGridEmailDispatcher... read sendgrid account details from config, use REST API to invoke
            //note: https://github.com/sendgrid/sendgrid-csharp
            //note: https://sendgrid.com/docs/API_Reference/SMTP_API/substitution_tags.html

            throw new NotImplementedException();
        }
    }
}
