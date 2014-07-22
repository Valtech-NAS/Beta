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

    public class ApplicationRepository : GenericMongoClient<MongoApplicationDetail>, IApplicationWriteRepository
    {
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
            Collection.Remove(Query<MongoApplicationDetail>.EQ(o => o.Id, id));
        }

        public ApplicationDetail Save(ApplicationDetail entity)
        {
            var mongoEntity = _mapper.Map<ApplicationDetail, MongoApplicationDetail>(entity);

            Collection.Save(mongoEntity);

            return _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }
    }
}
