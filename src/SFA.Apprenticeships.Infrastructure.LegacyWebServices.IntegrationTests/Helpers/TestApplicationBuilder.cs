namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;

    public class TestApplicationBuilder
    {
        private readonly Guid _entityid = new Guid();
        private int _vacancyId = 12345;
        private ApplicationTemplate _candidateInformation;

        public ApplicationDetail Build()
        {
            if (_candidateInformation == null)
            {
                _candidateInformation = new ApplicationTemplate
                {
                    AboutYou = new AboutYou(),
                    EducationHistory = null,
                    Qualifications = new Qualification[]
                    {
                    },
                    WorkExperience = new WorkExperience[]
                    {
                    }
                };
            }
            return new ApplicationDetail
            {
                EntityId = _entityid,
                Vacancy = new VacancySummary
                {
                    Id = _vacancyId
                },
                CandidateInformation = _candidateInformation
            };
        }

        public TestApplicationBuilder WithCandidateInformation()
        {
            _candidateInformation = new ApplicationTemplate
            {
                AboutYou = CreateFakeAboutYou(),
                EducationHistory = CreateFakeEducationHistory(),
                Qualifications = CreateFakeQualifications(),
                WorkExperience = CreateFakeWorkExperience()
            };

            return this;
        }

        public TestApplicationBuilder WithVacancyId(int vacancyId)
        {
            _vacancyId = vacancyId;
            return this;
        }

        public static ApplicationDetail CreateFakeApplicationDetail()
        {
            return new ApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Vacancy = new VacancySummary
                {
                    Id = 12345 // legacy vacancy id
                },
                CandidateInformation = new ApplicationTemplate
                {
                    AboutYou = CreateFakeAboutYou(),
                    EducationHistory = CreateFakeEducationHistory(),
                    Qualifications = CreateFakeQualifications(),
                    WorkExperience = CreateFakeWorkExperience()
                }
            };
        }

        public static ApplicationDetail CreateFakeMinimalApplicationDetail()
        {
            return new ApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                Vacancy = new VacancySummary
                {
                    Id = 12345 // legacy vacancy id
                },
                CandidateInformation = new ApplicationTemplate
                {
                    AboutYou = null,
                    EducationHistory = null,
                    Qualifications = new Qualification[]
                    {
                    },
                    WorkExperience = new WorkExperience[]
                    {
                    }
                }
            };
        }


        private static AboutYou CreateFakeAboutYou()
        {
            return new AboutYou
            {
                Strengths = "Strengths",
                Improvements = "Improvements",
                HobbiesAndInterests = "HobbiesAndInterests",
                Support = "Support"
            };
        }

        private static Education CreateFakeEducationHistory()
        {
            return new Education
            {
                Institution = "Bash Street School",
                FromYear = 2008,
                ToYear = 2010
            };
        }

        private static List<WorkExperience> CreateFakeWorkExperience()
        {
            return new List<WorkExperience>
            {
                new WorkExperience
                {
                    Employer = "Acme Corp",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = new DateTime(2012, 1, 1),
                    Description = "Barista"
                },
                new WorkExperience
                {
                    Employer = "Nether Products",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = new DateTime(2012, 1, 1),
                    Description = "Cashier"
                },
                new WorkExperience
                {
                    Employer = "Argos",
                    FromDate = new DateTime(2011, 1, 1),
                    ToDate = DateTime.MinValue,
                    Description = "Counter Staff"
                }
            };
        }

        private static List<Qualification> CreateFakeQualifications()
        {
            return new List<Qualification>
            {
                new Qualification
                {
                    Subject = "English",
                    Year = 2009,
                    Grade = "A",
                    IsPredicted = false,
                    QualificationType = "GCSE"
                },
                new Qualification
                {
                    Subject = "Maths",
                    Year = 2010,
                    Grade = "C",
                    IsPredicted = true,
                    QualificationType = "A Level"
                }
            };
        }
    }
}
