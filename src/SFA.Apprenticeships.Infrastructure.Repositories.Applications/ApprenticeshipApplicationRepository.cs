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

    public class ApprenticeshipApprenticeshipApprenticeshipApplicationRepository : GenericMongoClient<MongoApprenticeshipApplicationDetail>, IApprenticeshipApplicationReadRepository,
        IApprenticeshipApplicationWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;

        public ApprenticeshipApprenticeshipApprenticeshipApplicationRepository(
            IConfigurationManager configurationManager,
            IMapper mapper)
            : base(configurationManager, "Applications.mongoDB", "apprenticeships")
        {
            _mapper = mapper;
        }

        public void Delete(Guid id)
        {
            Logger.Debug("Calling repository to delete ApprenticeshipApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoApprenticeshipApplicationDetail>.EQ(o => o.Id, id));

            Logger.Debug("Deleted ApprenticeshipApplicationDetail with Id={0}", id);
        }

        public ApprenticeshipApplicationDetail Save(ApprenticeshipApplicationDetail entity)
        {
            Logger.Debug("Calling repository to save ApprenticeshipApplicationDetail Id={0}, Status={1}", entity.EntityId, entity.Status);

            var mongoEntity = _mapper.Map<ApprenticeshipApplicationDetail, MongoApprenticeshipApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved ApprenticeshipApplicationDetail to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public ApprenticeshipApplicationDetail Get(Guid id, bool errorIfNotFound)
        {
            Logger.Debug("Calling repository to get ApprenticeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            string message;

            if (mongoEntity == null && errorIfNotFound)
            {
                 message = string.Format("Unknown ApprenticeshipApplicationDetail with Id={0}", id);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            message = mongoEntity == null ? "Found no ApprenticeshipApplicationDetail with Id={0}" : "Found ApprenticeshipApplicationDetail with Id={0}";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public ApprenticeshipApplicationDetail Get(int legacyApplicationId)
        {
            Logger.Debug("Calling repository to get ApprenticeshipApplicationDetail with legacy Id={0}", legacyApplicationId);

            var mongoEntity = Collection.AsQueryable().FirstOrDefault(a => a.LegacyApplicationId == legacyApplicationId);

            var message = mongoEntity == null ? "Found no ApprenticeshipApplicationDetail with legacy Id={0}" : "Found ApprenticeshipApplicationDetail with legacy Id={0}";

            Logger.Debug(message, legacyApplicationId);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public IList<ApprenticeshipApplicationSummary> GetForCandidate(Guid candidateId)
        {
            Logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);
            
            // Get apprenticeship application summaries for the specified candidate, excluding any that are archived.
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            Logger.Debug("{0} MongoApprenticeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            Logger.Debug("Mapping MongoApprenticeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper
                .Map<MongoApprenticeshipApplicationDetail[], IEnumerable<ApprenticeshipApplicationSummary>>(mongoApplicationDetailsList)
                .ToList();

            Logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetailsList.Count(), candidateId);

            return applicationDetailsList;
        }

        public ApprenticeshipApplicationDetail GetForCandidate(Guid candidateId, Func<ApprenticeshipApplicationDetail, bool> filter)
        {
            Logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);

            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            Logger.Debug("{0} MongoApprenticeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            Logger.Debug("Mapping MongoApprenticeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper.Map<MongoApprenticeshipApplicationDetail[], IEnumerable<ApprenticeshipApplicationDetail>>(
                mongoApplicationDetailsList);

            var applicationDetails = applicationDetailsList as IList<ApprenticeshipApplicationDetail> ?? applicationDetailsList.ToList();

            Logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetails.Count(), candidateId);

            return applicationDetails
                .Where(filter)
                .FirstOrDefault(); // we expect zero or 1
        }

        public ApprenticeshipApplicationDetail Get(Guid id)
        {
            Logger.Debug("Calling repository to get ApprenticeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no ApprenticeshipApplicationDetail with Id={0}" : "Found ApprenticeshipApplicationDetail with Id={0}";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipApplicationDetail, ApprenticeshipApplicationDetail>(mongoEntity);
        }

        public void ExpireOrWithdrawForCandidate(Guid candidateId, int vacancyId)
        {
            Logger.Debug("Calling repository to expire or withdraw apprenticeship application for candidate with Id={0} and VacancyId={1}", candidateId, vacancyId);

            var applicationDetail = GetForCandidate(
                candidateId, each => each.Vacancy.Id == vacancyId);

            if (applicationDetail == null)
            {
                Logger.Debug("Apprenticeship application to be expired or withdrawn was not found for CandidateId={0}, VacancyId={1}", candidateId, vacancyId);
                return;
            }

            Logger.Debug("Found apprenticeship application to be expired or withdrawn with Id={0}, Status={1}", applicationDetail.EntityId, applicationDetail.Status);

            applicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;

            Save(applicationDetail);

            Logger.Debug("Saved expired or withdrawn apprenticeship application for candidate with Id={0} and VacancyId={1}", applicationDetail.CandidateId, vacancyId);
        }
    }
}
