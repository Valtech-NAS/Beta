namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using NLog;

    public class ApplicationRepository : GenericMongoClient<MongoApplicationDetail>, IApplicationReadRepository,
        IApplicationWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;

        public ApplicationRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Applications.mongoDB", "applications")
        {
            _mapper = mapper;
        }

        public void Delete(Guid id)
        {
            Logger.Debug("Called Mongodb to delete ApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoApplicationDetail>.EQ(o => o.Id, id));
        }

        public ApplicationDetail Save(ApplicationDetail entity)
        {
            Logger.Debug("Called Mongodb to save ApplicationDetail EntityId={0}, Status={1}", entity.EntityId, entity.Status);

            var mongoEntity = _mapper.Map<ApplicationDetail, MongoApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved ApplicationDetail to Mongodb with Id={0}", entity.EntityId);

            return _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }

        public ApplicationDetail Get(Guid id)
        {
            Logger.Debug("Called Mongodb to get ApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);
            return mongoEntity == null ? null : _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }

        public ApplicationDetail Get(Expression<Func<ApplicationDetail, bool>> filter)
        {
            //var item = Collection.AsQueryable().Where(filter);
            //todo: return the first application that matches the filter
            // .FirstOrDefault()
            throw new NotImplementedException();
        }

        public IList<ApplicationSummary> GetForCandidate(Guid candidateId)
        {
            //todo: retrieve applications for the specified candidate, should exclude any that are archived
            throw new NotImplementedException();
        }

        public ApplicationDetail GetForCandidate(Guid candidateId, Func<ApplicationDetail, bool> filter)
        {
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            var applicationDetailsList = _mapper.Map<MongoApplicationDetail[], IEnumerable<ApplicationDetail>>(
                mongoApplicationDetailsList);

            return applicationDetailsList
                .Where(filter)
                .SingleOrDefault(); // we expect zero or 1
        }
    }
}
