namespace SFA.Apprenticeships.Application.Services.CommunicationService
{
    using System;
    using System.Text;
    using Common.AppSettings;
    using Domain.Entities;
    using Interfaces.Communications;

    public class CommunciationService : ICommunciationService
    {
        private readonly IEmailDispatcher _emailDispatcher;
        public CommunciationService(IEmailDispatcher emailDispatcher)
        {
            _emailDispatcher = emailDispatcher;
        }

        public void SubmitEnquiry(EmployerEnquiry message)
        {
            string toEmailAddress = BaseAppSettingValues.ToEmailAddress;
            string fromEmailAddress = message.Email;
            string fromName = message.Firstname;
            bool isDemoModeEnabled = BaseAppSettingValues.IsDemoModeEnabled;

            EmailRequest emailRequest = new EmailRequest
            {
                FromEmail = fromEmailAddress,
                FromName = fromName,
                ToEmail = isDemoModeEnabled ? message.Email : toEmailAddress, //todo: temporary set to driven by flag IsDemoModeEnabled for testing purposes
                Subject = string.Format("Enquiry from applicant '{0} {1}' at {2} on {3}", message.Firstname.ToUpper(), message.Lastname.ToUpper(), DateTime.Now.ToShortTimeString(), DateTime.Now.ToShortDateString()) //todo : check for the preferred subject format
            };

            #region Build the email content
            //todo: replace this with some html template file
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Full name : {0}", message.Firstname));
            builder.AppendLine(string.Format("Email : {0}", message.Email));
            builder.AppendLine(string.Format("Position at company : {0}", message.Position));
            builder.AppendLine(string.Format("Phone number : {0}", message.WorkPhoneNumber));

            builder.AppendLine(string.Format("Address: Address line 1 - {0}", message.ApplicantAddress.AddressLine1));

            if (!string.IsNullOrEmpty(message.ApplicantAddress.AddressLine2))
            {
                builder.AppendLine(string.Format("Address: Address line 2 - {0}", message.ApplicantAddress.AddressLine2));
            }
            if (!string.IsNullOrEmpty(message.ApplicantAddress.AddressLine3))
            {
                builder.AppendLine(string.Format("Address: Address line 3 - {0}", message.ApplicantAddress.AddressLine3));
            }
            if (!string.IsNullOrEmpty(message.ApplicantAddress.City))
            {
                builder.AppendLine(string.Format("Address: Address line 4 - {0}", message.ApplicantAddress.City));
            }

            builder.AppendLine(string.Format("Address: Postcode - {0}", message.ApplicantAddress.Postcode));
            builder.AppendLine(string.Format("Total no of employees : {0}", message.EmployeesCount));
            builder.AppendLine(string.Format("Industry sector : {0}", message.WorkSector));
            builder.AppendLine(string.Format("Enquiry Description : {0}", message.EnquiryDescription));
            #endregion

            emailRequest.EmailContent = builder.ToString();

            _emailDispatcher.SendEmail(emailRequest);
        }
    }
}