namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Applications
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class ApprenticeshipApplicationWriteRepositoryTests
    {
        [Test, Category("Integration")]
        public void ShouldCreateAndDeleteApplication()
        {
            // arrange
#pragma warning disable 0618
            // TODO: AG: CRITICAL: NuGet package update on 2014-10-30.
            var writer = ObjectFactory.GetInstance<IApprenticeshipApplicationWriteRepository>();
            var reader = ObjectFactory.GetInstance<IApprenticeshipApplicationReadRepository>();
#pragma warning restore 0618

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
