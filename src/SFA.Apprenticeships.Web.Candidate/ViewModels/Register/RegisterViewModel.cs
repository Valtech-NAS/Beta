namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Register
{
    using Locations;

    public class RegisterViewModel
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public short Day { get; set; }

        public short Month { get; set; }

        public short Year { get; set; }

        public AddressViewModel Address{ get; set; }

        public string EmailAddress { get; set; }

        public bool CanContactByPhone { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public bool HasAcceptedTermsAndConditions { get; set; }
    }
}