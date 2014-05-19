using System;
using System.Security.Policy;
using SFA.Apprenticeships.Common.Interfaces.Enums;

namespace SFA.Apprenticeships.Common.Entities.Vacancy
{
    public class VacancyDetail : VacancySummary
    {
        public string ContractOwner { get; set; }
        public string DeliveryOrganisation { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsite { get; set; }
        public string ExpectedDuration { get; set; }
        public string FullDescription { get; set; }
        public string FutureProspects { get; set; }
        public string OtherInformation { get; set; }

        public DateTime InterviewFromDate { get; set; }
        public DateTime StartDate { get; set; }

        public bool IsDisplayRecruitmentAgency { get; set; }
        public bool IsSmallEmployerWageIncentive { get; set; }
        public int ProviderSectorPassRate { get; set; }
        public string ProviderDescription { get; set; }
        public string TrainingToBeProvided { get; set; }

        public string PersonalQualities { get; set; }
        public string QualificationRequired { get; set; }
        public string SkillsRequired { get; set; }

        public string SupplementaryQuestion1 { get; set; }
        public string SupplementaryQuestion2 { get; set; }

        public string VacancyManager { get; set; }
        public string VacancyOwner { get; set; }

        public double Wage { get; set; }
        public string WageDescription { get; set; }
        public WageType TypeOfWage { get; set; }
        public string WorkingWeek { get; set; }
    }
}
