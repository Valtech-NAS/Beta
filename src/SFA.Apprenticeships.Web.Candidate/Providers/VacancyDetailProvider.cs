namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using ViewModels.VacancySearch;

    public class VacancyDetailProvider : IVacancyDetailProvider
    {
        private readonly IVacancyDataService _vacancyDataService;
        private readonly IMapper _vacancyDetailMapper;

        public VacancyDetailProvider(IVacancyDataService vacancyDataService, IMapper vacancyDetailMapper)
        {
            _vacancyDataService = vacancyDataService;
            _vacancyDetailMapper = vacancyDetailMapper;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(int id)
        {
            var vacancyDetail = _vacancyDataService.GetVacancyDetails(id);

            if (vacancyDetail == null)
            {
                return null;
            }

            var vacancyDetailViewModel = _vacancyDetailMapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);

            return vacancyDetailViewModel;
        }
    }
}
