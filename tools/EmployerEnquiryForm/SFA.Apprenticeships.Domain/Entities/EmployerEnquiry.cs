namespace SFA.Apprenticeships.Domain.Entities
{
    public class EmployerEnquiry
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Position { get; set; }
        public string WorkTelephonNumber { get; set; }
        public string Mobile { get; set; }
        public string EmployeesCount { get; set; }
        public Address ApplicantAddress { get; set; }
        public string WorkSector { get; set; }
        public string PreviousExperienceType { get; set; }
        public string EnquiryDescription { get; set; }
        public string EnquirySource { get; set; }
    }
}