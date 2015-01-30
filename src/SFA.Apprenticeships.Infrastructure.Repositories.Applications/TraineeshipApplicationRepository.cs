namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    public class TraineeshipApplicationRepository : GenericMongoClient<MongoTraineeshipApplicationDetail>, ITraineeshipApplicationReadRepository, ITraineeshipApplicationWriteRepository
    {
        private readonly ILogService _logger;

        private readonly IMapper _mapper;

        public TraineeshipApplicationRepository(
            IConfigurationManager configurationManager,
            IMapper mapper, ILogService logger)
            : base(configurationManager, "Applications.mongoDB", "traineeships")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete TraineeshipApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoTraineeshipApplicationDetail>.EQ(o => o.Id, id));

            _logger.Debug("Deleted TraineeshipApplicationDetail with Id={0}", id);
        }

        public TraineeshipApplicationDetail Save(TraineeshipApplicationDetail entity)
        {
            _logger.Debug("Calling repository to save TraineeshipApplicationDetail Id={0}", entity.EntityId);

            var mongoEntity = _mapper.Map<TraineeshipApplicationDetail, MongoTraineeshipApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved TraineeshipApplicationDetail to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public TraineeshipApplicationDetail Get(Guid id, bool errorIfNotFound)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            string message;

            if (mongoEntity == null && errorIfNotFound)
            {
                message = string.Format("Unknown TraineeshipApplicationDetail with Id={0}", id);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with Id={0}" : "Found TraineeshipApplicationDetail with Id={0}";

            _logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public TraineeshipApplicationDetail Get(int legacyApplicationId)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationDetail with legacy Id={0}", legacyApplicationId);

            var mongoEntity = Collection.AsQueryable().FirstOrDefault(a => a.LegacyApplicationId == legacyApplicationId);

            var message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with legacy Id={0}" : "Found TraineeshipApplicationDetail with legacy Id={0}";

            _logger.Debug(message, legacyApplicationId);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public IList<TraineeshipApplicationSummary> GetForCandidate(Guid candidateId)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationSummary list for candidate with Id={0}", candidateId);

            // Get traineeship application summaries for the specified candidate, excluding any that are archived.
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            _logger.Debug("{0} MongoTraineeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            _logger.Debug("Mapping MongoTraineeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper
                .Map<MongoTraineeshipApplicationDetail[], IEnumerable<TraineeshipApplicationSummary>>(mongoApplicationDetailsList)
                .ToList();

            _logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetailsList.Count(), candidateId);

            return applicationDetailsList;
        }

        public IEnumerable<TraineeshipApplicationSummary> GetApplicationSummaries(int vacancyId)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationSummaries with VacancyId:{0}", vacancyId);

            var mongoEntities = Collection.Find(Query.EQ("Vacancy._id", vacancyId)).ToArray();

            _logger.Debug("Found {0} TraineeshipApplicationSummaries with VacancyId:{1}", mongoEntities.Count(), vacancyId);

            var applicationSummaries =
                _mapper.Map<IEnumerable<MongoTraineeshipApplicationDetail>, IEnumerable<TraineeshipApplicationSummary>>(
                    mongoEntities);

            return applicationSummaries;
        }

        public TraineeshipApplicationDetail GetForCandidate(Guid candidateId, int vacancyId, bool errorIfNotFound = false)
        {
            _logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);

            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId && each.Vacancy.Id == vacancyId)
                .ToArray();

            if (mongoApplicationDetailsList.Length == 0 && errorIfNotFound)
            {
                var message = string.Format("No TraineeshipApplicationDetail found for candidateId={0} and vacancyId={1}", candidateId, vacancyId);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            _logger.Debug("{0} MongoTraineeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            _logger.Debug("Mapping MongoTraineeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper.Map<MongoTraineeshipApplicationDetail[], IEnumerable<TraineeshipApplicationDetail>>(
                mongoApplicationDetailsList);

            var applicationDetails = applicationDetailsList as IList<TraineeshipApplicationDetail> ?? applicationDetailsList.ToList();

            _logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetails.Count(), candidateId);

            return applicationDetails
                .FirstOrDefault(); // we expect zero or 1
        }

        public TraineeshipApplicationDetail Get(Guid id)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with Id={0}" : "Found TraineeshipApplicationDetail with Id={0}";

            _logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }
    }
}
