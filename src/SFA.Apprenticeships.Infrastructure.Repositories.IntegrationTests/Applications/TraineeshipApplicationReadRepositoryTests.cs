﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Applications
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FizzWare.NBuilder;
    using FluentAssertions;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Repositories.Applications.Entities;

    [TestFixture]
    public class TraineeshipApplicationReadRepositoryTests : RepositoryIntegrationTest
    {
        private IConfigurationManager _configurationManager;
        private ITraineeshipApplicationReadRepository _traineeshipApplicationReadRepository;
        private ITraineeshipApplicationWriteRepository _traineeshipApplicationWriteRepository;
        private MongoDatabase _database;
        private MongoCollection<MongoTraineeshipApplicationDetail> _collection;

        private const int TestVacancyId = -100;

        [SetUp]
        public void SetUp()
        {
            _configurationManager = Container.GetInstance<IConfigurationManager>();
            _traineeshipApplicationReadRepository = Container.GetInstance<ITraineeshipApplicationReadRepository>();
            _traineeshipApplicationWriteRepository = Container.GetInstance<ITraineeshipApplicationWriteRepository>();

            var mongoConnectionString = _configurationManager.GetAppSetting("Applications.mongoDB");
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
            _collection = _database.GetCollection<MongoTraineeshipApplicationDetail>("traineeships");
        }

        [TearDown]
        public void TearDown()
        {
            _collection.Remove(Query.EQ("Vacancy._id", TestVacancyId));
        }

        [Test, Category("Integration")]
        public void GetTraineeshipApplicationsReturnsCorrectResults()
        {
            //Arrange - Build Applications
            var applications = BuildApprenticeshipApplicationDetails();
            applications.ForEach(a => _traineeshipApplicationWriteRepository.Save(a));

            //Act - Get application vacancy statuses
            var summaries = _traineeshipApplicationReadRepository
                .GetApplicationSummaries(TestVacancyId)
                .ToList();

            //Assert - the correct number of applicaitons with the correct vacancy state
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Live).Should().Be(10);
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Unavailable).Should().Be(7);
            summaries.Count(v => v.VacancyStatus == VacancyStatuses.Expired).Should().Be(6);
        }

        private static List<TraineeshipApplicationDetail> BuildApprenticeshipApplicationDetails()
        {
            var vacancyStatus =
                Builder<TraineeshipApplicationDetail>.CreateListOfSize(23)
                    .All()
                    .With(a => a.Vacancy.Id = TestVacancyId)
                    .TheFirst(10)
                    .With(a => a.VacancyStatus = VacancyStatuses.Live)
                    .TheNext(7)
                    .With(a => a.VacancyStatus = VacancyStatuses.Unavailable)
                    .TheNext(6)
                    .With(a => a.VacancyStatus = VacancyStatuses.Expired)
                    .Build().ToList();

            return vacancyStatus.ToList();
        } 
    }
}
