namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;

    public class TraineeshipApplicationDetailToTraineeshipApplicationViewModelResolver :
        ITypeConverter<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModel Convert(ResolutionContext context)
        {
            var application = (TraineeshipApplicationDetail) context.SourceValue;

            var model = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel().Resolve(application),
                DateUpdated = application.DateUpdated,
                VacancyId = application.Vacancy.Id,
                Status = application.Status
            };

            return model;
        }
    }
}