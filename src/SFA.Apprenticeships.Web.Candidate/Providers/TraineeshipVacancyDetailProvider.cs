namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;
    using NLog;
    using SFA.Apprenticeships.Application.Interfaces.Vacancies;
    using SFA.Apprenticeships.Domain.Entities.Exceptions;
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    using SFA.Apprenticeships.Domain.Interfaces.Mapping;
    using SFA.Apprenticeships.Web.Candidate.Constants.Pages;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;

    public class TraineeshipVacancyDetailProvider : ITraineeshipVacancyDetailProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;
        private readonly IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail> _vacancySearchService;

        public TraineeshipVacancyDetailProvider(
            IMapper mapper,
            IVacancySearchService<TraineeshipSummaryResponse, TraineeshipVacancyDetail> vacancySearchService)
        {
            _mapper = mapper;
            _vacancySearchService = vacancySearchService;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(Guid? candidateId, int vacancyId)
        {
            Logger.Debug(
                "Calling TraineeshipVacancyDetailProvider to get the Vacancy detail View Model for candidate ID: {0}, vacancy ID: {1}.",
                candidateId, vacancyId);

            try
            {
                var vacancyDetail = _vacancySearchService.GetVacancyDetails(vacancyId);

                if (vacancyDetail == null)
                {
                    return null;
                }

                var vacancyDetailViewModel = _mapper.Map<TraineeshipVacancyDetail, VacancyDetailViewModel>(vacancyDetail);

                return vacancyDetailViewModel;
            }
            catch (CustomException e)
            {
                var message = string.Format("Get Traineeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);

                return new VacancyDetailViewModel(TraineeshipVacancyDetailPageMessages.GetVacancyDetailFailed);
            }
            catch (Exception e)
            {
                var message = string.Format("Get Traineeship Vacancy View Model failed for candidate ID: {0}, vacancy ID: {1}.",
                    candidateId, vacancyId);

                Logger.Error(message, e);
                throw;
            }
        }
    }
}