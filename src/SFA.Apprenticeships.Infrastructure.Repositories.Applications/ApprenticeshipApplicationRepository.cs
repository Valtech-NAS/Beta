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

    public class ApprenticeshipApplicationRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipApplicationReadRepository,
        IApprenticeshipApplicationWriteRepository
    {
        private readonly ILogService _logger;

        private readonly IMapper _mapper;

        public ApprenticeshipApplicationRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "Applications.mongoDB", "apprenticeships")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete ApprenticeshipApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoApprenticeshipApplicationDetail>.EQ(o => o.Id, id));

            _logger.Debug("Deleted ApprenticeshipApplicationDetail with Id={0}", id);
        }

        public ApprenticeshipApplicationDetail Save(ApprenticeshipApplicationDetail entity)
        {
            _logger.Debug("Calling repository to save ApprenticeshipApplicationDetail Id={0}, Status={1}", entity.EntityId, entity.Status);

            var mongoEntity = _mapper.Map<ApprenticeshipApplicationDetail, MongoApprenticeshipApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved ApprenticeshipApplicationDetail to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public ApprenticeshipApplicationDetail Get(Guid id, bool errorIfNotFound)
        {
            _logger.Debug("Calling repository to get ApprenticeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            string message;

            if (mongoEntity == null && errorIfNotFound)
            {
                 message = string.Format("Unknown ApprenticeshipApplicationDetail with Id={0}", id);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            message = mongoEntity == null ? "Found no ApprenticeshipApplicationDetail with Id={0}" : "Found ApprenticeshipApplicationDetail with Id={0}";

            _logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public ApprenticeshipApplicationDetail Get(int legacyApplicationId)
        {
            _logger.Debug("Calling repository to get ApprenticeshipApplicationDetail with legacy Id={0}", legacyApplicationId);

            var mongoEntity = Collection.AsQueryable().SingleOrDefault(a => a.LegacyApplicationId == legacyApplicationId);

            var message = mongoEntity == null ? "Found no ApprenticeshipApplicationDetail with legacy Id={0}" : "Found ApprenticeshipApplicationDetail with legacy Id={0}";

            _logger.Debug(message, legacyApplicationId);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public IList<ApprenticeshipApplicationSummary> GetForCandidate(Guid candidateId)
        {
            _logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);
            
            // Get apprenticeship application summaries for the specified candidate, excluding any that are archived.
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            _logger.Debug("{0} MongoApprenticeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            _logger.Debug("Mapping MongoApprenticeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper
                .Map<MongoApprenticeshipApplicationDetail[], IEnumerable<ApprenticeshipApplicationSummary>>(mongoApplicationDetailsList)
                .ToList();

            _logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetailsList.Count(), candidateId);

            return applicationDetailsList;
        }

        public ApprenticeshipApplicationDetail GetForCandidate(Guid candidateId, int vacancyId, bool errorIfNotFound = false)
        {
            _logger.Debug("Calling repository to get ApplicationSummary for candidateId={0} and vacancyId={1}", candidateId, vacancyId);

            var mongoApplicationDetail = Collection.AsQueryable().SingleOrDefault(each => each.CandidateId == candidateId && each.Vacancy.Id == vacancyId);

            if (mongoApplicationDetail == null && errorIfNotFound)
            {
                var message = string.Format("No ApprenticeshipApplicationDetail found for candidateId={0} and vacancyId={1}", candidateId, vacancyId);
                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            _logger.Debug("Returning ApplicationSummary for candidateId={0} and vacancyId={1}", candidateId, vacancyId);

            return _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoApplicationDetail);
        }

        public IEnumerable<ApprenticeshipApplicationSummary> GetApplicationSummaries(int vacancyId)
        {
            _logger.Debug("Calling repository to get ApprenticeshipApplicationSummaries with VacancyId:{0}", vacancyId);

            var mongoEntities = Collection.Find(Query.EQ("Vacancy._id", vacancyId)).ToArray();

            _logger.Debug("Found {0} ApprenticeshipApplicationSummaries with VacancyId:{1}", mongoEntities.Count(), vacancyId);

            var applicationSummaries =
                _mapper.Map<IEnumerable<MongoApprenticeshipApplicationDetail>, IEnumerable<ApprenticeshipApplicationSummary>>(
                    mongoEntities);

            return applicationSummaries;
        }

        public ApprenticeshipApplicationDetail Get(Guid id)
        {
            _logger.Debug("Calling repository to get ApprenticeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no ApprenticeshipApplicationDetail with Id={0}" : "Found ApprenticeshipApplicationDetail with Id={0}";

            _logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public void ExpireOrWithdrawForCandidate(Guid candidateId, int vacancyId)
        {
            _logger.Debug("Calling repository to expire or withdraw apprenticeship application for candidate with Id={0} and VacancyId={1}", candidateId, vacancyId);

            var applicationDetail = GetForCandidate(candidateId, vacancyId);

            if (applicationDetail == null)
            {
                _logger.Debug("Apprenticeship application to be expired or withdrawn was not found for CandidateId={0}, VacancyId={1}", candidateId, vacancyId);
                return;
            }

            _logger.Debug("Found apprenticeship application to be expired or withdrawn with Id={0}, Status={1}", applicationDetail.EntityId, applicationDetail.Status);

            applicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;

            Save(applicationDetail);

            _logger.Debug("Saved expired or withdrawn apprenticeship application for candidate with Id={0} and VacancyId={1}", applicationDetail.CandidateId, vacancyId);
        }
    }
}
