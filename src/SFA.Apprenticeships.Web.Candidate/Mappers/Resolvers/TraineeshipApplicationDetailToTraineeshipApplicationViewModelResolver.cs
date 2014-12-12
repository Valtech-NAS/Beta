namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Linq;
    using AutoMapper;
    using Domain.Entities.Applications;
    using Helpers;
    using ViewModels.Applications;
    using ViewModels.Candidate;

    public class TraineeshipApplicationDetailToTraineeshipApplicationViewModelResolver :
        ITypeConverter<ApplicationDetail, TraineeshipApplicationViewModel>
    {
        public TraineeshipApplicationViewModel Convert(ResolutionContext context)
        {
            var application = (ApplicationDetail) context.SourceValue;

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