namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Applications;
    using Entities;

    public class ApplicationDetailMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApplicationDetail, MongoApplicationDetail>();
            Mapper.CreateMap<MongoApplicationDetail, ApplicationDetail>();
            Mapper.CreateMap<MongoApplicationDetail, ApplicationSummary>();
        }
    }
}
