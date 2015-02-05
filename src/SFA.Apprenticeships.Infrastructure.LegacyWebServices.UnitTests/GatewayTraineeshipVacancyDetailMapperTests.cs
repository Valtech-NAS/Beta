﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.UnitTests
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Traineeships;
    using FluentAssertions;
    using GatewayServiceProxy;
    using Mappers.Traineeship;
    using NUnit.Framework;

    [TestFixture]
    public class GatewayTraineeshipVacancyDetailMapperTests
    {
        private LegacyTraineeshipVacancyDetailMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new LegacyTraineeshipVacancyDetailMapper();
        }

        [TestCase]
        public void ShouldCreateAMap()
        {
            // Act.
            _mapper.Mapper.AssertConfigurationIsValid();
        }

        [TestCase]
        [ExpectedException(typeof(AutoMapperMappingException))]
        public void ShouldThrowIfNotTraineeship()
        {
            // Arrange.
            var src = new Vacancy
            {
                VacancyType = "Not a Traineeship",
            };

            // Act.
            _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert: throws.
        }

        [TestCase]
        public void ShouldMapAllOneToOneFields()
        {
            // Arrange.
            var src = new Vacancy
            {
                VacancyId =  67,
                VacancyReference = 42,
                VacancyType = "Traineeship",
                Status = "Live",
                ApplicationInstructions = "ApplicationInstructions",
                ClosingDate = DateTime.Today.AddDays(1),
                ContactPerson = "ContactPerson",
                ContractedProviderName = "ContractedProviderName",
                ContractOwner = "ContractOwner",
                DeliveryOrganisation = "DeliveryOrganisation",
                EmployerAnonymousDescription = "EmployerAnonymousDescription",
                EmployerDescription = "EmployerDescription",
                EmployerName = "EmployerName",
                EmployerWebsite = "EmployerWebsite",
                ExpectedDuration = "ExpectedDuration",
                FullDescription = "FullDescription",
                FutureProspects = "FutureProspects",
                ImportantOtherInfo = "ImportantOtherInfo",
                InterviewFromDate = DateTime.Today.AddDays(2),
                NumberOfPositions = 101,
                PersonalQualities = "PersonalQualities",
                PossibleStartDate = DateTime.Today.AddDays(3),
                QualificationRequired = "QualificationRequired",
                RealityCheck = "RealityCheck",
                ShortDescription = "ShortDescription",
                SkillsRequired = "SkillsRequired",
                SupplementaryQuestion1 = "SupplementaryQuestion1",
                SupplementaryQuestion2 = "SupplementaryQuestion2",
                TradingName = "TradingName",
                TrainingProviderDesc = "TrainingProviderDesc",
                TrainingRequired = "TrainingRequired",
                VacancyManager = "VacancyManager",
                VacancyOwner = "VacancyOwner",
                VacancyTitle = "VacancyTitle",
                VacancyUrl = "VacancyUrl",
                WageType = "Weekly",
                WorkingWeek = "WorkingWeek"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Id.Should().Be(src.VacancyId);
            dest.VacancyStatus.Should().Be(VacancyStatuses.Live);
            dest.VacancyReference.Should().Be("VAC" + src.VacancyReference.ToString("D9"));
            dest.ApplicationInstructions.Should().Be(src.ApplicationInstructions);
            dest.ClosingDate.Should().Be(src.ClosingDate);
            dest.Contact.Should().Be(src.ContactForCandidate);
            dest.ProviderName.Should().Be(src.ContractedProviderName);
            dest.ContractOwner.Should().Be(src.ContractOwner);
            dest.DeliveryOrganisation.Should().Be(src.DeliveryOrganisation);
            dest.AnonymousEmployerName.Should().Be(src.EmployerAnonymousDescription);
            dest.EmployerDescription.Should().Be(src.EmployerDescription);
            dest.EmployerName.Should().Be(src.EmployerName);
            dest.EmployerWebsite.Should().Be(src.EmployerWebsite);
            dest.ExpectedDuration.Should().Be(src.ExpectedDuration);
            dest.FullDescription.Should().Be(src.FullDescription);
            dest.FutureProspects.Should().Be(src.FutureProspects);
            dest.OtherInformation.Should().Be(src.ImportantOtherInfo);
            dest.InterviewFromDate.Should().Be(src.InterviewFromDate);
            dest.NumberOfPositions.Should().Be(src.NumberOfPositions);
            dest.PersonalQualities.Should().Be(src.PersonalQualities);
            dest.StartDate.Should().Be(src.PossibleStartDate);
            dest.QualificationRequired.Should().Be(src.QualificationRequired);
            dest.RealityCheck.Should().Be(src.RealityCheck);
            dest.Description.Should().Be(src.ShortDescription);
            dest.SkillsRequired.Should().Be(src.SkillsRequired);
            dest.SupplementaryQuestion1.Should().Be(src.SupplementaryQuestion1);
            dest.SupplementaryQuestion2.Should().Be(src.SupplementaryQuestion2);
            dest.TradingName.Should().Be(src.TradingName);
            dest.ProviderDescription.Should().Be(src.TrainingProviderDesc);
            dest.TrainingToBeProvided.Should().Be(src.TrainingRequired);
            dest.VacancyManager.Should().Be(src.VacancyManager);
            dest.VacancyOwner.Should().Be(src.VacancyOwner);
            dest.Title.Should().Be(src.VacancyTitle);
            dest.VacancyUrl.Should().Be(src.VacancyUrl);
            dest.WorkingWeek.Should().Be(src.WorkingWeek);
            dest.WageType.Should().Be(WageType.Weekly);
        }

        [TestCase]
        public void ShouldMapApplyViaEmployerWebsiteWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                ApplyViaEmployerWebsite = true,
                ApplyViaEmployerWebsiteSpecified = false,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ApplyViaEmployerWebsite.Should().Be(false);
        }

        [TestCase]
        public void ShouldMapApplyViaEmployerWebsiteWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                ApplyViaEmployerWebsite = true,
                ApplyViaEmployerWebsiteSpecified = true,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ApplyViaEmployerWebsite.Should().Be(true);
        }

        [TestCase]
        public void ShouldMapEmployerAnonymousWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                EmployerAnonymous = true,
                EmployerAnonymousSpecified = false,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.IsEmployerAnonymous.Should().Be(false);
        }

        [TestCase]
        public void ShouldMapEmployerAnonymousWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                EmployerAnonymous = true,
                EmployerAnonymousSpecified = true,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.IsEmployerAnonymous.Should().Be(true);
        }

        [TestCase]
        public void ShouldMapApprFrameworkSuccessRateWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                ApprFrameworkSuccessRate = 42,
                ApprFrameworkSuccessRateSpecified = false,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ProviderSectorPassRate.Should().Be(null);
        }

        [TestCase]
        public void ShouldMapApprFrameworkSuccessRateWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                ApprFrameworkSuccessRate = 42,
                ApprFrameworkSuccessRateSpecified = true,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.ProviderSectorPassRate.Should().Be(42);
        }

        [TestCase]
        public void ShouldMapVacancyAddressWhenNotSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                VacancyAddress = null,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyAddress.Should().NotBeNull();

            dest.VacancyAddress.AddressLine1.Should().BeNull();
            dest.VacancyAddress.AddressLine1.Should().BeNull();
            dest.VacancyAddress.AddressLine1.Should().BeNull();
            dest.VacancyAddress.AddressLine1.Should().BeNull();

            dest.VacancyAddress.Postcode.Should().BeNull();
            dest.VacancyAddress.Uprn.Should().BeNull();

            dest.VacancyAddress.GeoPoint.Should().NotBeNull();
            dest.VacancyAddress.GeoPoint.Latitude.Should().Be(0.0);
            dest.VacancyAddress.GeoPoint.Longitude.Should().Be(0.0);
        }


        [TestCase]
        public void ShouldMapVacancyAddressWhenSpecified()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                VacancyAddress = new AddressDetails
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLine3",
                    AddressLine4 = "AddressLine4",
                    AddressLine5 = "AddressLine5",
                    Town = "Town",
                    County = "County",
                    PostCode = "Postcode",
                    LatitudeSpecified = true,
                    Latitude = 1.0m,
                    LongitudeSpecified = true,
                    Longitude = 2.0m,
                },
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyAddress.Should().NotBeNull();

            dest.VacancyAddress.AddressLine1.Should().Be("AddressLine1");
            dest.VacancyAddress.AddressLine2.Should().Be("AddressLine2, AddressLine3, AddressLine4, AddressLine5");
            dest.VacancyAddress.AddressLine3.Should().Be("Town");
            dest.VacancyAddress.AddressLine4.Should().Be("County");

            dest.VacancyAddress.Postcode.Should().Be("Postcode");
            dest.VacancyAddress.Uprn.Should().BeNull();

            dest.VacancyAddress.GeoPoint.Should().NotBeNull();
            dest.VacancyAddress.GeoPoint.Latitude.Should().Be(1.0);
            dest.VacancyAddress.GeoPoint.Longitude.Should().Be(2.0);
        }

        [TestCase("Live", VacancyStatuses.Live)]
        [TestCase("Posted In Error", VacancyStatuses.Unavailable)]
        [TestCase("PostedInError", VacancyStatuses.Unavailable)]
        [TestCase("Withdrawn", VacancyStatuses.Unavailable)]
        [TestCase("Deleted", VacancyStatuses.Unavailable)]
        [TestCase("Closed", VacancyStatuses.Expired)]
        [TestCase("Completed", VacancyStatuses.Expired)]
        public void ShouldMapVacancyStatus(string vacancyStatusString, VacancyStatuses vacancyStatus)
        {
            // Arrange.
            var src = new Vacancy
            {
                VacancyType = "Traineeship",
                Status = vacancyStatusString,
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyStatus.Should().Be(vacancyStatus);
        }

        [TestCase]
        [ExpectedException(typeof(AutoMapperMappingException))]
        public void ShouldThrowIfUnknownVacancyStatus()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Wrong",
                VacancyType = "Traineeship",
                WageType = "Weekly"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert: exception expected.
        }

        [TestCase]
        [ExpectedException(typeof(AutoMapperMappingException))]
        public void ShouldThrowIfUnknownWageType()
        {
            // Arrange.
            var src = new Vacancy
            {
                Status = "Live",
                VacancyType = "Traineeship",
                WageType = "Wrong"
            };

            // Act.
            var dest = _mapper.Map<Vacancy, TraineeshipVacancyDetail>(src);

            // Assert: exception expected.
        }
    }
}