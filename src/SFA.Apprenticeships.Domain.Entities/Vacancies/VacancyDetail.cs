namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System;
    using Locations;

    public class VacancyDetail
    {
        #region Vacancy

        public int Id { get; set; }

        public string VacancyReference { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FullDescription { get; set; }

        public string Framework { get; set; }

        public VacancyLevel VacancyLevel { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ClosingDate { get; set; }

        public DateTime InterviewFromDate { get; set; }

        public decimal Wage { get; set; }

        public string WageDescription { get; set; }

        public WageType WageType { get; set; }

        public string WorkingWeek { get; set; }

        public string OtherInformation { get; set; }

        public string FutureProspects { get; set; }

        public string VacancyOwner { get; set; }

        public string VacancyManager { get; set; }

        public VacancyLocationType VacancyLocationType { get; set; }

        public VacancyType VacancyType { get; set; }

        public string LocalAuthority { get; set; }

        public int NumberOfPositions { get; set; }

        public string RealityCheck { get; set; }

        public DateTime Created { get; set; }

        #endregion

        #region Employer

        public string EmployerName { get; set; }

        public string AnonymousEmployerName { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerWebsite { get; set; }

        public bool ApplyViaEmployerWebsite { get; set; }

        public string VacancyUrl { get; set; }

        public string ApplicationInstructions { get; set; }

        public bool IsEmployerAnonymous { get; set; }

        public string ExpectedDuration { get; set; }

        public Address VacancyAddress { get; set; }

        public bool IsRecruitmentAgencyAnonymous { get; set; }

        public bool IsSmallEmployerWageIncentive { get; set; }

        public string SupplementaryQuestion1 { get; set; }

        public string SupplementaryQuestion2 { get; set; }

        #endregion

        #region Provider

        public string ProviderName { get; set; }

        public string TradingName { get; set; }

        public string ProviderDescription { get; set; }

        public string Contact { get; set; }

        public int? ProviderSectorPassRate { get; set; }

        public string TrainingToBeProvided { get; set; }

        public string ContractOwner { get; set; }

        public string DeliveryOrganisation { get; set; }

        public bool IsNasProvider { get; set; }

        #endregion

        #region Candidate

        public string PersonalQualities { get; set; }

        public string QualificationRequired { get; set; }

        public string SkillsRequired { get; set; }

        #endregion 
    }
}
