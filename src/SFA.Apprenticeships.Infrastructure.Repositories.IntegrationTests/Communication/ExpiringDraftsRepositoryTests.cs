namespace SFA.Apprenticeships.Infrastructure.Repositories.IntegrationTests.Communication
{
    using System;
    using System.Linq;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FizzWare.NBuilder;
    using FluentAssertions;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Repositories.Communication.Entities;

    [TestFixture]
    public class ExpiringDraftsRepositoryTests : RepositoryIntegrationTest
    {
        private IConfigurationManager _configurationManager;
        private IExpiringDraftRepository _expiringDraftRepository;
        private MongoDatabase _database;
        private MongoCollection<MongoExpiringDraft> _collection;

        private const int TestVacancyId = -200;

        [SetUp]
        public void SetUp()
        {
            _configurationManager = Container.GetInstance<IConfigurationManager>();
            _expiringDraftRepository = Container.GetInstance<IExpiringDraftRepository>();

            var mongoConnectionString = _configurationManager.GetAppSetting("Communications.mongoDB");
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            _database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
            _collection = _database.GetCollection<MongoExpiringDraft>("expiringdrafts");
        }

        [TearDown]
        public void TearDown()
        {
            _collection.Remove(Query.EQ("VacancyId", TestVacancyId));
        }

        [Test, Category("Integration")]
        public void TestMultiSaveGetAndDeleteCandidatesDailyDigest()
        {
            //Arrange
            var batchId = Guid.NewGuid();
            var sentDateTime = DateTime.Now;
            var expiringDrafts =
                Builder<ExpiringDraft>.CreateListOfSize(3)
                    .All()
                    .With(ed => ed.VacancyId = TestVacancyId)
                    .With(ed => ed.BatchId = batchId)
                    .With(ed => ed.SentDateTime = sentDateTime)
                    .Build().ToList();

            //Act
            expiringDrafts.ForEach(ed =>
            {
                _expiringDraftRepository.Save(ed);
                ed.BatchId = null;
                ed.SentDateTime = null;
                _expiringDraftRepository.Save(ed);
            });

            
            //Assert
            var candidatesDailyDigest = _expiringDraftRepository.GetCandidatesDailyDigest();
            candidatesDailyDigest.Count().Should().Be(3);
            var returnedExpiringDrafts = candidatesDailyDigest.SelectMany(cand => cand.Value.ToArray());
            returnedExpiringDrafts.Count(ed => ed.VacancyId == TestVacancyId && ed.BatchId == null && ed.SentDateTime == null)
                .Should()
                .Be(3);

            //Act
            expiringDrafts.ForEach(_expiringDraftRepository.Delete);

            //Assert
            candidatesDailyDigest = _expiringDraftRepository.GetCandidatesDailyDigest();
            candidatesDailyDigest.Count().Should().Be(0);
        } 
    }
}
