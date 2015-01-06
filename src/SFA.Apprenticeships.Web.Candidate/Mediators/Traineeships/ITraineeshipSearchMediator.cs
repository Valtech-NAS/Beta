namespace SFA.Apprenticeships.Web.Candidate.Mediators.Traineeships
{
    using System;
    using ViewModels.VacancySearch;

    public interface ITraineeshipSearchMediator
    {
        MediatorResponse<TraineeshipSearchViewModel> Index();

        MediatorResponse<TraineeshipSearchResponseViewModel> Results(TraineeshipSearchViewModel model);

        MediatorResponse<VacancyDetailViewModel> Details(int vacancyId, Guid? candidateId, string searchReturnUrl);
    }
}