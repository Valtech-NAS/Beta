using System.Collections.Generic;
using SFA.Apprenticeships.Application.Candidate.Strategies;
using SFA.Apprenticeships.Application.Vacancy;
using SFA.Apprenticeships.Domain.Entities.Applications;
using SFA.Apprenticeships.Domain.Entities.Candidates;
using SFA.Apprenticeships.Domain.Entities.Locations;
using SFA.Apprenticeships.Domain.Entities.Vacancies;
using SFA.Apprenticeships.Infrastructure.Repositories.Applications;
using SFA.Apprenticeships.Infrastructure.Repositories.Applications.Mappers;
using SFA.Apprenticeships.Infrastructure.Repositories.Candidates;
using SFA.Apprenticeships.Infrastructure.Repositories.Candidates.Mappers;

namespace SampleDataManufacturer
{
    using System;
    using SFA.Apprenticeships.Application.UserAccount.Strategies;
    using SFA.Apprenticeships.Domain.Entities.Users;
    using SFA.Apprenticeships.Domain.Interfaces.Configuration;
    using SFA.Apprenticeships.Infrastructure.Common.Configuration;
    using SFA.Apprenticeships.Infrastructure.Repositories.Users;
    using SFA.Apprenticeships.Infrastructure.Repositories.Users.Mappers;
    using SFA.Apprenticeships.Infrastructure.UserDirectory;
    using SFA.Apprenticeships.Infrastructure.UserDirectory.ActiveDirectory;
    using SFA.Apprenticeships.Infrastructure.UserDirectory.Configuration;

    internal class DataManufacturer
    {
        private const string ActivationCode = "ABC123";
        private const string Password = "?Password01!";
        private readonly IConfigurationManager _configurationManager;
        private readonly UserRepository _userRepository;
        private readonly CandidateRepository _candidateWriteRepository;
        private readonly IVacancyDataProvider _vacancyDataProvider;
        private readonly ApplicationRepository _applicationRepository;

        internal DataManufacturer()
        {
            _configurationManager = new ConfigurationManager();
            var userMappers = new UserMappers();
            _userRepository = new UserRepository(_configurationManager, userMappers);

            var candidateMappers = new CandidateMappers();
            _candidateWriteRepository = new CandidateRepository(_configurationManager, candidateMappers);
            _vacancyDataProvider = new FakeVacancyDataProvider();

            var applicationMappers = new ApplicationMappers();
            _applicationRepository = new ApplicationRepository(_configurationManager, applicationMappers);
        }

        internal Guid CreateUser()
        {
            var registerUserStrategy = new RegisterUserStrategy(_userRepository, _configurationManager, _userRepository);
            var activateUserStrategy = new ActivateUserStrategy(_userRepository, _userRepository);
            var adServer = new ActiveDirectoryServer(ActiveDirectoryConfiguration.Instance);
            var userDirectoryProvider = new ActiveDirectoryUserDirectoryProvider(adServer, null);

            var username = GetUsername();
            var userId = Guid.NewGuid();
            userDirectoryProvider.CreateUser(userId.ToString(), Password);
            registerUserStrategy.Register(username, userId, ActivationCode, UserRoles.Candidate);
            activateUserStrategy.Activate(username, ActivationCode);

            var newCandidate = CreateFakeCandidate(userId, username);
            _candidateWriteRepository.Save(newCandidate);

            return userId;
        }

        public void CreateApplication(Guid candidateId, int vacancyId)
        {
            var createApplicationStrategy = new CreateApplicationStrategy(_vacancyDataProvider, _applicationRepository,
                _applicationRepository, _candidateWriteRepository);

            createApplicationStrategy.CreateApplication(candidateId, vacancyId);
        }

        private string GetUsername()
        {
            return string.Format("nas.exemplar+{0}@gmail.com", DateTime.Now.Ticks);
        }

        private static Candidate CreateFakeCandidate(Guid candidateId, string emailAddress)
        {
            return new Candidate
            {
                ApplicationTemplate = new ApplicationTemplate
                {
                    AboutYou = CreateFakeAboutYou(),
                    EducationHistory = CreateFakeEducationHistory(),
                    Qualifications = CreateFakeQualifications(),
                    WorkExperience = CreateFakeWorkExperience()
                },
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                EntityId = candidateId,
                LegacyCandidateId = 1,
                RegistrationDetails = CreateFakeRegistrationDetails(emailAddress)
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

        private static RegistrationDetails CreateFakeRegistrationDetails(string emailAddress)
        {
            return new RegistrationDetails
            {
                FirstName = "NAS",
                LastName = "Exemplar",
                EmailAddress = emailAddress,
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
            };
        }
    }

    internal class FakeVacancyDataProvider : IVacancyDataProvider
    {
        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            return new VacancyDetail
            {
                EmployerName = "Acme Corp",
                IsEmployerAnonymous = false,
                WageType = WageType.Weekly,
                Wage = 101.19m,
                ApplyViaEmployerWebsite = false
            };
        }
    }
}
