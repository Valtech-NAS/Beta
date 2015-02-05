﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Candidates
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using NUnit.Framework;

    [TestFixture]
    public class CandidateRepositoryTests : RepositoryIntegrationTest
    {
        [Test, Category("Integration")]
        public void ShouldCreateUpdateAndRemoveCandidate()
        {
            // arrange
            var writer = Container.GetInstance<ICandidateWriteRepository>();
            var reader = Container.GetInstance<ICandidateReadRepository>();

            var candidate = CreateTestCandidate();

            // act, assert
            var savedCandidate = writer.Save(candidate);
            Assert.AreEqual(candidate.RegistrationDetails.FirstName, savedCandidate.RegistrationDetails.FirstName);

            // act, assert
            savedCandidate.RegistrationDetails.FirstName = "Lois";
            savedCandidate = writer.Save(savedCandidate);
            Assert.AreEqual("Lois", savedCandidate.RegistrationDetails.FirstName);

            // act, assert
            writer.Delete(savedCandidate.EntityId);
            Assert.IsNull(reader.Get(savedCandidate.EntityId));
        }

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
                    EmailAddress = string.Format("peter+{0}@griffin.net", DateTime.Now.Ticks),
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
                    EducationHistory = new Education
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
                        new WorkExperience { Employer = "Some employer", JobTitle = "Beer Tester", Description = "Tested beer at the brewery", FromDate = new DateTime(2000, 1, 1), ToDate = new DateTime(2001, 12, 31) },
                        new WorkExperience { Employer = "Another employer", JobTitle = "Barman", Description = "Served drinks and swept up behind the bar", FromDate = new DateTime(2002, 1, 1), ToDate = new DateTime(2002, 12, 31) }
                    }
                }
            };

            return candidate;
        }

        #endregion
    }
}
