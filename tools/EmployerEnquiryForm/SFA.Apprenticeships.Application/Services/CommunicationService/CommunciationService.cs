namespace SFA.Apprenticeships.Application.Services.CommunicationService
{
    using System;
    using System.Text;
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
            EmailRequest emailRequest = new EmailRequest
            {
                ToEmail = message.Email,
                Subject = string.Format("Enquiry from applicant at {0}", DateTime.Now.ToLongDateString()) //todo : check for the preferred subject format
            };

            #region Build the email content
            //todo: replace this with some html template file
            var builder = new StringBuilder();
            builder.Append(string.Format("First Name : {0}", message.FirstName));
            builder.AppendLine(string.Format("Sur Name : {0}", message.SurName));
            builder.AppendLine(string.Format("Email : {0}", message.Email));
            builder.AppendLine(string.Format("Position : {0}", message.Position));
            builder.AppendLine(string.Format("Work TelephonNumber : {0}", message.WorkTelephonNumber));
            builder.AppendLine(string.Format("Mobile : {0}", message.Mobile));
            builder.AppendLine(string.Format("Address: AddressLine1 - {0}, AddressLine2 - {1}, AddressLine3 - {2}, County - {3}, City - {4}, Country - {5}, Postcode - {6}",
                                              message.ApplicantAddress.AddressLine1,
                                              message.ApplicantAddress.AddressLine2,
                                              message.ApplicantAddress.AddressLine3,
                                              message.ApplicantAddress.County,
                                              message.ApplicantAddress.City,
                                              message.ApplicantAddress.Country,
                                              message.ApplicantAddress.Postcode));

            builder.AppendLine(string.Format("Employees Count : {0}", message.EmployeesCount));
            builder.AppendLine(string.Format("Work Sector : {0}", message.WorkSector));
            builder.AppendLine(string.Format("Previous Experience Type : {0}", message.PreviousExperienceType));
            builder.AppendLine(string.Format("Enquiry Description : {0}", message.EnquiryDescription));
            builder.AppendLine(string.Format("Enquiry Source : {0}", message.EnquirySource)); 
            #endregion
            
            emailRequest.EmailContent = builder.ToString();
            
            _emailDispatcher.SendEmail(emailRequest);
        }
    }
}