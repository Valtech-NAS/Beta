namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using NLog;

    public class ApplicationRepository : GenericMongoClient<MongoApplicationDetail>, IApplicationWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IMapper _mapper;

        public ApplicationRepository(IConfigurationManager configurationManager, IMapper mapper)
            : base(configurationManager, "Applications.mongoDB", "applications")
        {
            _mapper = mapper;
        }

        /* TODO: Map legacy application statuses
                 Unsent
                 Sent
                 In progress
                 Withdrawn
                 Unsuccessful
                 Successful
                 Past Application
         */

        public void Delete(Guid id)
        {
            Logger.Debug("Called Mongodb to delete ApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoApplicationDetail>.EQ(o => o.Id, id));
        }

        public ApplicationDetail Save(ApplicationDetail entity)
        {
            Logger.Debug("Called Mongodb to save ApplicationDetail EntityId={0}, Status={1}", entity.EntityId, entity.Status);
            
            var mongoEntity = _mapper.Map<ApplicationDetail, MongoApplicationDetail>(entity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved ApplicationDetail to Mongodb with Id={0}", entity.EntityId);

            return _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }
    }
}
