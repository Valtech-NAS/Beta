namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.Collections.Generic;
    using Locations;

    [Serializable]
    public class CandidateViewModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public AddressViewModel Address { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<QualificationsViewModel> Qualifications { get; set; }

        public IEnumerable<WorkExperienceViewModel> WorkExperience { get; set; }

        public AboutYouViewModel AboutYou { get; set; }

        public EmployerQuestionAnswersViewModel  EmployerQuestionAnswers { get; set; }

    }
}