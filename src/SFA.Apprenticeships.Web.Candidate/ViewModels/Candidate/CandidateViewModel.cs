namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.Collections.Generic;
    using Locations;

    [Serializable]
    public class CandidateViewModel
    {
      
        public Guid Id { get; set; }

        public string FullName
        {
            get
            {
                if (FirstName == null || LastName == null)
                {
                    return null;
                }

                return FirstName + " " + LastName;
            }
        } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } //TODO CHECK do we need this here

        public DateTime DateOfBirth { get; set; }

        public AddressViewModel Address { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public EducationViewModel Education { get; set; }

        public IEnumerable<QualificationsViewModel> Qualifications { get; set; }

        public IEnumerable<WorkExperienceViewModel> WorkExperience { get; set; }

        public AboutYouViewModel AboutYou { get; set; }

        public EmployerQuestionAnswersViewModel  EmployerQuestionAnswers { get; set; }

    }
}