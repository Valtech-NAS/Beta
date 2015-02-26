namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System.Net;
    using System.Net.Mail;
    using Application.Interfaces.Communications;
    using Common.AppSettings;
    using Domain.Entities;
    using SendGrid;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {

        public SendGridEmailDispatcher()
        {
            
        }

        public void SendEmail(EmailRequest request)
        {
            // Create the email object first, then add the properties.
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(request.ToEmail);
            myMessage.From = new MailAddress(request.FromEmail, request.FromName);
            myMessage.Subject = request.Subject;
            myMessage.Text = request.EmailContent;

            // Create credentials, specifying your network user name and password.
            string networkUsername = BaseAppSettingValues.NetworkUsername;
            string networkPassword = BaseAppSettingValues.NetworkPassword;
            var credentials = new NetworkCredential(networkUsername, networkPassword);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            transportWeb.Deliver(myMessage);
        }
    }
}