namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications.IoC
{
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class ApplicationRepositoryRegistry : Registry
    {
        public ApplicationRepositoryRegistry()
        {
            // Apprenticeships.
            For<IMapper>().Use<ApprenticeshipApplicationMappers>().Name = "ApplicationDetailMapper";

            For<IApprenticeshipApplicationWriteRepository>().Use<ApprenticeshipApprenticeshipApprenticeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("ApplicationDetailMapper");

            For<IApprenticeshipApplicationReadRepository>()
                .Use<ApprenticeshipApprenticeshipApprenticeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("ApplicationDetailMapper");

            // Traineeships.
            For<IMapper>().Use<TraineeshipApplicationMappers>().Name = "TraineeshipApplicationDetailMapper";

            For<ITraineeshipApplicationWriteRepository>().Use<TraineeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("TraineeshipApplicationDetailMapper");

            For<ITraineeshipApplicationReadRepository>()
                .Use<TraineeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("TraineeshipApplicationDetailMapper");
        }
    }
}
