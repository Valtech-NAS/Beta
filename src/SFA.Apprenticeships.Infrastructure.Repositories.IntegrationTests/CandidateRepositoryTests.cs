namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using System;
    using Candidates.IoC;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class CandidateRepositoryTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<CandidateRepositoryRegistry>();
                x.AddRegistry<CommonRegistry>();
            });
        }

        [Test]
        public void ShouldCreateUpdateAndRemoveCandidate()
        {
            // arrange
            var writer = ObjectFactory.GetInstance<ICandidateWriteRepository>();
            var reader = ObjectFactory.GetInstance<ICandidateReadRepository>();
            var candidate = CreateTestCandidate();

            // act, assert
            var savedCandidate = writer.Save(candidate);
            Assert.AreEqual(candidate.FirstName, savedCandidate.FirstName);

            // act, assert
            savedCandidate.FirstName = "Lois";
            savedCandidate = writer.Save(savedCandidate);
            Assert.AreEqual("Lois", savedCandidate.FirstName);

            // act, assert
            writer.Delete(savedCandidate.Id);
            Assert.IsNull(reader.Get(savedCandidate.Id));
        }

        #region Helpers
        private static Candidate CreateTestCandidate()
        {
            return new Candidate
            {
                Id = Guid.NewGuid(),
                FirstName = "Peter",
                MiddleNames = string.Empty,
                LastName = "Griffin",
                Address = new Address
                {
                    AddressLine1 = "31 Spooner Street",
                    AddressLine2 = "Quahog",
                    AddressLine3 = string.Empty,
                    AddressLine4 = "Rhode Island",
                    Postcode = "CV1 2WT"
                },
                PhoneNumber = "555 123 4567",
                EmailAddress = "peter@griffin.net",
                DateOfBirth = new DateTime(1970, 3, 1),
                Roles = UserRoles.Candidate
            };
        }

        #endregion
    }
}
