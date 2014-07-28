namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
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
            throw new NotImplementedException();
        }

        public IList<ApplicationSummary> GetForCandidate(Guid candidateId)
        {
            //todo: retrieve vacancies for the specified candidate, should exclude any that are archived
            throw new NotImplementedException();
        }
    }
}
