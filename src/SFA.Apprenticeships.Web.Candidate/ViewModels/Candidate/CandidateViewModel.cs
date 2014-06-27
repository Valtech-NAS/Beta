namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Locations;

    public class CandidateViewModel
    {
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public AddressViewModel Address { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<QualificationsViewModel> Qualifications { get; set; }

        public IEnumerable<WorkExperienceViewModel> WorkExperience { get; set; }

        public AboutYouViewModel AboutYou { get; set; }

    }
}