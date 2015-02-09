namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using System;
    using ViewModels.VacancySearch;

    public interface ITraineeshipSearchMediator
    {
        MediatorResponse<TraineeshipSearchViewModel> Index();

        MediatorResponse<TraineeshipSearchResponseViewModel> Results(TraineeshipSearchViewModel model);

        MediatorResponse<VacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId, string searchReturnUrl);
    }
}