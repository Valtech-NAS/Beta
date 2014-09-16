namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using System;
    using Applications.IoC;
    using Common.IoC;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ApplicationRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }

        [Test, Category("Integration")]
        public void ShouldCreateApplication()
        {
            // arrange
            var writer = ObjectFactory.GetInstance<IApplicationWriteRepository>();
            var application = CreateTestApplication();

            // act, assert
            var savedApplication = writer.Save(application);
        }

        #region Helpers
        private static ApplicationDetail CreateTestApplication()
        {
            return new ApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = 12345,
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
