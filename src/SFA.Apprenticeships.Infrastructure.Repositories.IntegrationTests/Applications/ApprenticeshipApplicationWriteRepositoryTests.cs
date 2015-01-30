namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Applications
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Repositories.Applications.Entities;

    [TestFixture]
    public class ApprenticeshipApplicationWriteRepositoryTests : RepositoryIntegrationTest
    {
        private const int LegacyApplicationId = 12345;

        [SetUp]
        public void SetUp()
        {
            var configurationManager = Container.GetInstance<IConfigurationManager>();
            var mongoConnectionString = configurationManager.GetAppSetting("Applications.mongoDB");
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
            
            var collection = database.GetCollection<MongoApprenticeshipApplicationDetail>("apprenticeships");
            collection.Remove(Query.EQ("LegacyApplicationId", LegacyApplicationId));
        }

        [Test, Category("Integration")]
        public void ShouldCreateAndDeleteApplication()
        {
            // arrange
            var writer = Container.GetInstance<IApprenticeshipApplicationWriteRepository>();
            var reader = Container.GetInstance<IApprenticeshipApplicationReadRepository>();
            
            
            var application = CreateTestApplication();

            // act
            writer.Save(application);

            //assert
            var savedApplication = reader.Get(application.LegacyApplicationId);
            savedApplication.Should().NotBeNull();
            savedApplication.EntityId.Should().Be(application.EntityId);
            savedApplication.CandidateDetails.FirstName.Should().Be(application.CandidateDetails.FirstName);
            savedApplication.CandidateDetails.Address.AddressLine1.Should().Be(application.CandidateDetails.Address.AddressLine1);

            writer.Delete(savedApplication.EntityId);
            savedApplication = reader.Get(application.LegacyApplicationId);
            savedApplication.Should().BeNull();
        }

        #region Helpers
        private static ApprenticeshipApplicationDetail CreateTestApplication()
        {
            return new ApprenticeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = LegacyApplicationId,
                VacancyStatus = VacancyStatuses.Live,
                CandidateDetails =
                {
                    FirstName = "Johnny",
                    LastName = "Candidate",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    EmailAddress = "email@server.com",
                    PhoneNumber = "07777111222",
                    Address =
                    {
                        AddressLine1 = "Address line 1",
                        AddressLine2 = "Address line 2",
                        AddressLine3 = "Address line 3",
                        AddressLine4 = "Address line 4",
                        Postcode = "CV1 2WT"
                    }
                },
                CandidateInformation =
                {
                    AboutYou =
                    {
                        HobbiesAndInterests = "Hobbies and interests",
                        Improvements = "Improvements are not needed",
                        Strengths = "My strengths are many",
                        Support = "Third line"
                    }
                }
            };
        }

        #endregion
    }
}
