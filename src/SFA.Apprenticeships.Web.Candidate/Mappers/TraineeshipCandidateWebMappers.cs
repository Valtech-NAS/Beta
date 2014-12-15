namespace SFA.Apprenticeships.Web.Candidate.Mappers
{
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Infrastructure.Common.Mappers;
    using SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;

    public class TraineeshipCandidateWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<TraineeshipApplicationViewModel, TraineeshipApplicationDetail>()
                .ConvertUsing<TraineeeshipApplicationViewModelToTraineeeshipApplicationDetailResolver>();

            Mapper.CreateMap<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>()
                .ConvertUsing<TraineeshipApplicationDetailToTraineeshipApplicationViewModelResolver>();
        }
    }
}