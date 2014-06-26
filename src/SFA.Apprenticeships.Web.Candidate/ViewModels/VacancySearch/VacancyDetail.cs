using SFA.Apprenticeships.Domain.Entities.Locations;

namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;
    using Domain.Entities.Locations;

    public class VacancyDetailViewModel
    {
        #region Vacancy

        public int Id { get; set; }

        public string FullVacancyReferenceId
        {
            get { return "VAC" + Id.ToString("D9"); }
        }

        public string Title { get; set; }
        
        public string Description { get; set; }

        public string FullDescription { get; set; }

        public string Framework { get; set; }
        
        public string VacancyType { get; set; }

        public DateTime ClosingDate { get; set; }

        public double Wage { get; set; }

        public string WageDescription { get; set; }

        public string WorkingWeek { get; set; }

        #endregion

        #region Employer

        public string EmployerName { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerWebsite { get; set; }

        public string ExpectedDuration { get; set; }

        public Address VacancyAddress { get; set; }

        #endregion

        #region Provider

        public string ProviderName { get; set; }

        public string Contact { get; set; }

        public int? ProviderSectorPassRate { get; set; }

        public string TrainingToBeProvided { get; set; }

        #endregion

        #region Candidate

        public string QualificationRequired { get; set; }

        public string SkillsRequired { get; set; }
        
        public string FutureProspects { get; set; }

        #endregion        
    }
}
