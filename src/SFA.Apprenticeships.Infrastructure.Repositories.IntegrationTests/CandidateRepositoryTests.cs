namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests
{
    using System;
    using Candidates.IoC;
    using Common.IoC;
    using Domain.Entities.Candidates;
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

        //todo: why are these commented out?
        [Test]
        public void ShouldCreateUpdateAndRemoveCandidate()
        {
            // arrange
            var writer = ObjectFactory.GetInstance<ICandidateWriteRepository>();
            var reader = ObjectFactory.GetInstance<ICandidateReadRepository>();
            var candidate = CreateTestCandidate();

            // act, assert
            var savedCandidate = writer.Save(candidate);
            //Assert.AreEqual(candidate.PersonalDetails.FirstName, savedCandidate.PersonalDetails.FirstName);

            //// act, assert
            //savedCandidate.PersonalDetails.FirstName = "Lois";
            //savedCandidate = writer.Save(savedCandidate);
            //Assert.AreEqual("Lois", savedCandidate.PersonalDetails.FirstName);

            //// act, assert
            //writer.Delete(savedCandidate.EntityId);
            //Assert.IsNull(reader.Get(savedCandidate.EntityId));
        }

        //todo: assert update of create and update timestamps for create, update, save of entity

        #region Helpers
        private static Candidate CreateTestCandidate()
        {
            var candidate = new Candidate
            {
                EntityId = Guid.NewGuid(),
                LegacyCandidateId = 12345,
                RegistrationDetails =
                {
                    FirstName = "Peter",
                    MiddleNames = string.Empty,
                    LastName = "Griffin",
                    Address = 
                    {
                        AddressLine1 = "31 Spooner Street",
                        AddressLine2 = "Quahog",
                        AddressLine3 = string.Empty,
                        AddressLine4 = "Rhode Island",
                        Postcode = "CV1 2WT"
                    },
                    PhoneNumber = "555 123 4567",
                    EmailAddress = "peter@griffin.net",
                    DateOfBirth = new DateTime(1970, 3, 1)
                },
                ApplicationTemplate =
                {
                    AboutYou =
                    {
                        HobbiesAndInterests = "Socialising",
                        Improvements = "None",
                        Strengths = "Sense of humour",
                        Support = "Sturdy chair"
                    },
                    EducationHistory =
                    {
                        FromYear = 1987, ToYear = 1997, Institution = "Some school"
                    },
                    Qualifications =
                    {
                        new Qualification { QualificationType = "GCSE", Subject = "Maths", Grade = "A", Year = 2000, IsPredicted = false },
                        new Qualification { QualificationType = "GCSE", Subject = "English", Grade = "A", Year = 2000, IsPredicted = false },
                        new Qualification { QualificationType = "GCSE", Subject = "Physics", Grade = "A", Year = 2000, IsPredicted = false },
                        new Qualification { QualificationType = "GCSE", Subject = "Chemistry", Grade = "A", Year = 2000, IsPredicted = false },
                        new Qualification { QualificationType = "GCSE", Subject = "Music", Grade = "A", Year = 2000, IsPredicted = false }
                    },
                    WorkExperience =
                    {
                        new WorkExperience { Employer = "Some employer", JobTitle = "Beer Tester", Description = "Tested beer at the brewery", FromYear = 2000, ToYear = 2001 },
                        new WorkExperience { Employer = "Another employer", JobTitle = "Barman", Description = "Served drinks and swept up behind the bar", FromYear = 2002, ToYear = 2002 }
                    }
                }
            };

            return candidate;
        }

        #endregion
    }
}
