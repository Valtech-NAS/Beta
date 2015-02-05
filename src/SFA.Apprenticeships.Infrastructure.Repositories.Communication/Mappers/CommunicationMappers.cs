namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Communication;
    using Entities;

    public class CommunicationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseUserCredentialsMappers();
        }

        private void InitialiseUserCredentialsMappers()
        {
            Mapper.CreateMap<ExpiringApprenticeshipApplicationDraft, MongoApprenticeshipApplicationExpiringDraft>();
            Mapper.CreateMap<MongoApprenticeshipApplicationExpiringDraft, ExpiringApprenticeshipApplicationDraft>();
        }
    }
}