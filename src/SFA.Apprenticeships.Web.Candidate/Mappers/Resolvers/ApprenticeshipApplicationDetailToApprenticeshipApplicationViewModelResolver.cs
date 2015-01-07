namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using SFA.Apprenticeships.Domain.Entities.Applications;
    using SFA.Apprenticeships.Web.Candidate.Mappers.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Applications;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;

    public class ApprenticeshipApplicationDetailToApprenticeshipApplicationViewModelResolver :
        ITypeConverter<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>
    {
        public ApprenticeshipApplicationViewModel Convert(ResolutionContext context)
        {
            var application = (ApprenticeshipApplicationDetail) context.SourceValue;

            var model = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel().Resolve(application),
                DateUpdated = application.DateUpdated,
                VacancyId = application.Vacancy.Id,
                Status = application.Status
            };

            model.Candidate.AboutYou =
                ApplicationConverter.GetAboutYouViewModel(application.CandidateInformation.AboutYou);
            model.Candidate.Education =
                ApplicationConverter.GetEducationViewModel(application.CandidateInformation.EducationHistory);

            return model;
        }
    }
}