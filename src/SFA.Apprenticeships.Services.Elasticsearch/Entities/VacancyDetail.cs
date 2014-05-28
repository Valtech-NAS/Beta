namespace SFA.Apprenticeships.Services.Elasticsearch.Entities
{
    using System;
    using System.ComponentModel;

    [Description("VacancyFullData")]
    public class VacancyDetail : VacancySummary
    {
        [Description("ContactPerson")]
        public string Contact { get; set; }

        [Description("ContractOwner")]
        public string ContractOwner { get; set; }

        [Description("DeliveryOrganisation")]
        public string DeliveryOrganisation { get; set; }

        [Description("EmployerDescription")]
        public string EmployerDescription { get; set; }

        [Description("EmployerWebsite")]
        public string EmployerWebsite { get; set; }

        [Description("ExpectedDuration")]
        public string ExpectedDuration { get; set; }

        [Description("FullDescription")]
        public string FullDescription { get; set; }

        [Description("FutureProspects")]
        public string FutureProspects { get; set; }

        [Description("OtherImportantInformation")]
        public string OtherInformation { get; set; }

        [Description("InterviewFromDate")]
        public DateTime InterviewFromDate { get; set; }

        [Description("PossibleStartDate")]
        public DateTime StartDate { get; set; }

        [Description("IsDisplayRecruitmentAgency")]
        public bool IsDisplayRecruitmentAgency { get; set; }

        [Description("IsSmallEmployerWageIncentive")]
        public bool IsSmallEmployerWageIncentive { get; set; }

        [Description("LearningProviderSectorPassRate")]
        public int ProviderSectorPassRate { get; set; }

        [Description("LearningProviderDesc")]
        public string ProviderDescription { get; set; }

        [Description("TrainingToBeProvided")]
        public string TrainingToBeProvided { get; set; }

        [Description("PersonalQualities")]
        public string PersonalQualities { get; set; }

        [Description("QualificationRequired")]
        public string QualificationRequired { get; set; }

        [Description("SkillsRequired")]
        public string SkillsRequired { get; set; }

        [Description("SupplementaryQuestion1")]
        public string SupplementaryQuestion1 { get; set; }

        [Description("SupplementaryQuestion2")]
        public string SupplementaryQuestion2 { get; set; }

        [Description("VacancyManager")]
        public string VacancyManager { get; set; }

        [Description("VacancyOwner")]
        public string VacancyOwner { get; set; }

        [Description("Wage")]
        public double Wage { get; set; }

        [Description("WageText")]
        public string WageDescription { get; set; }

        [Description("WageType")]
        public WageType TypeOfWage { get; set; }

        [Description("WorkingWeek")]
        public string WorkingWeek { get; set; }
    }
}
