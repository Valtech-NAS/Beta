namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System;
    using System.Collections.Generic;
    using Locations;

    [Serializable]
    public abstract class CandidateViewModelBase
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

        public DateTime DateOfBirth { get; set; }

        public AddressViewModel Address { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public bool HasQualifications { get; set; }

        public IEnumerable<QualificationsViewModel> Qualifications { get; set; }

        public bool HasWorkExperience { get; set; }

        public IEnumerable<WorkExperienceViewModel> WorkExperience { get; set; }

        public EmployerQuestionAnswersViewModel  EmployerQuestionAnswers { get; set; }
    }
}