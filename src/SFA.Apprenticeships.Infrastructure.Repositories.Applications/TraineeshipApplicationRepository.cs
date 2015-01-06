namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using NLog;

    public class TraineeshipApplicationRepository : GenericMongoClient<MongoTraineeshipApplicationDetail>, ITraineeshipApplicationReadRepository, ITraineeshipApplicationWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;

        public TraineeshipApplicationRepository(
            IConfigurationManager configurationManager,
            IMapper mapper)
            : base(configurationManager, "Applications.mongoDB", "traineeships")
        {
            _mapper = mapper;
        }

        public void Delete(Guid id)
        {
            Logger.Debug("Calling repository to delete TraineeshipApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoTraineeshipApplicationDetail>.EQ(o => o.Id, id));

            Logger.Debug("Deleted TraineeshipApplicationDetail with Id={0}", id);
        }

        public TraineeshipApplicationDetail Save(TraineeshipApplicationDetail entity)
        {
            Logger.Debug("Calling repository to save TraineeshipApplicationDetail Id={0}", entity.EntityId);

            var mongoEntity = _mapper.Map<TraineeshipApplicationDetail, MongoTraineeshipApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved TraineeshipApplicationDetail to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public TraineeshipApplicationDetail Get(Guid id, bool errorIfNotFound)
        {
            Logger.Debug("Calling repository to get TraineeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            string message;

            if (mongoEntity == null && errorIfNotFound)
            {
                message = string.Format("Unknown TraineeshipApplicationDetail with Id={0}", id);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with Id={0}" : "Found TraineeshipApplicationDetail with Id={0}";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public TraineeshipApplicationDetail Get(int legacyApplicationId)
        {
            Logger.Debug("Calling repository to get TraineeshipApplicationDetail with legacy Id={0}", legacyApplicationId);

            var mongoEntity = Collection.AsQueryable().FirstOrDefault(a => a.LegacyApplicationId == legacyApplicationId);

            var message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with legacy Id={0}" : "Found TraineeshipApplicationDetail with legacy Id={0}";

            Logger.Debug(message, legacyApplicationId);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public IList<TraineeshipApplicationSummary> GetForCandidate(Guid candidateId)
        {
            Logger.Debug("Calling repository to get TraineeshipApplicationSummary list for candidate with Id={0}", candidateId);

            // Get traineeship application summaries for the specified candidate, excluding any that are archived.
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            Logger.Debug("{0} MongoTraineeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            Logger.Debug("Mapping MongoTraineeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper
                .Map<MongoTraineeshipApplicationDetail[], IEnumerable<TraineeshipApplicationSummary>>(mongoApplicationDetailsList)
                .ToList();

            Logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetailsList.Count(), candidateId);

            return applicationDetailsList;
        }

        public TraineeshipApplicationDetail GetForCandidate(Guid candidateId, int vacancyId, bool errorIfNotFound = false)
        {
            Logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);

            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId && each.Vacancy.Id == vacancyId)
                .ToArray();

            if (mongoApplicationDetailsList.Length == 0 && errorIfNotFound)
            {
                var message = string.Format("No TraineeshipApplicationDetail found for candidateId={0} and vacancyId={1}", candidateId, vacancyId);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            Logger.Debug("{0} MongoTraineeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            Logger.Debug("Mapping MongoTraineeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper.Map<MongoTraineeshipApplicationDetail[], IEnumerable<TraineeshipApplicationDetail>>(
                mongoApplicationDetailsList);

            var applicationDetails = applicationDetailsList as IList<TraineeshipApplicationDetail> ?? applicationDetailsList.ToList();

            Logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetails.Count(), candidateId);

            return applicationDetails
                .FirstOrDefault(); // we expect zero or 1
        }

        public TraineeshipApplicationDetail Get(Guid id)
        {
            Logger.Debug("Calling repository to get TraineeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with Id={0}" : "Found TraineeshipApplicationDetail with Id={0}";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }
    }
}
