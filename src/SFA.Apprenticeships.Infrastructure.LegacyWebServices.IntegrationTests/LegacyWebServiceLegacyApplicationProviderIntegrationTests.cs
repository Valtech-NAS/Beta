namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using Application.Candidate.Strategies;
    using Common.IoC;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LegacyWebServiceLegacyApplicationProviderIntegrationTests : ICandidateReadRepository
    {
        private ILegacyApplicationProvider _legacyApplicationProviderProvider;
        private ILegacyCandidateProvider _legacyCandidateProvider;

        public Candidate Get(Guid id)
        {
            var candidate = new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            };

            return candidate;
        }

        public Candidate Get(Guid id, bool errorIfNotFound)
        {
            throw new NotImplementedException();
        }

        public Candidate Get(string username, bool errorIfNotFound = true)
        {
            throw new NotImplementedException();
        }

        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LegacyWebServicesRegistry>();
                x.For<ICandidateReadRepository>().Use(this);
            });

            _legacyApplicationProviderProvider = ObjectFactory.GetInstance<ILegacyApplicationProvider>();
            _legacyCandidateProvider = ObjectFactory.GetInstance<ILegacyCandidateProvider>();
        }

        [Test]
        public void ShouldCreateApplication()
        {
            var applicationDetail = new ApplicationDetail
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

            var result = _legacyApplicationProviderProvider.CreateApplication(applicationDetail);

            result.Should().BeGreaterThan(0);
        }

        [Test]
        public void ShouldCreateApplicationForCandidateWithNoInformation()
        {
            var applicationDetail = new ApplicationDetail
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

            var result = _legacyApplicationProviderProvider.CreateApplication(applicationDetail);

            result.Should().BeGreaterThan(0);
        }

        private int CreateLegacyCandidateId()
        {
            var candidate = new Candidate
            {
                EntityId = Guid.NewGuid(),
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    PhoneNumber = "01683200911",
                    Address = new Address
                    {
                        AddressLine1 = "10 Acacia Avenue",
                        AddressLine3 = "Some House",
                        AddressLine4 = "Some Town",
                        Postcode = "FF2 7AL",
                        AddressLine2 = "East Nether"
                    },
                }
            };

            return _legacyCandidateProvider.CreateCandidate(candidate);
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
                    FromYear = 2011,
                    ToYear = 2012,
                    Description = "Barista"
                },
                new WorkExperience
                {
                    Employer = "Nether Products",
                    FromYear = 2011,
                    ToYear = 2012,
                    Description = "Cashier"
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