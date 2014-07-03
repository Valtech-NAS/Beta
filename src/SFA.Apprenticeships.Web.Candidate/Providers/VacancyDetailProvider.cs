namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Mapping;
    using ViewModels.VacancySearch;

    public class VacancyDetailProvider : IVacancyDetailProvider
    {
        private readonly IVacancyDataProvider _vacancyDataProvider;
        private readonly IMapper _vacancyDetailMapper;

        public VacancyDetailProvider(IVacancyDataProvider vacancyDataProvider, IMapper vacancyDetailMapper)
        {
            _vacancyDataProvider = vacancyDataProvider;
            _vacancyDetailMapper = vacancyDetailMapper;
        }

        public VacancyDetailViewModel GetVacancyDetailViewModel(int id)
        {
            VacancyDetail vacancyDetail = _vacancyDataProvider.GetVacancyDetails(id);

            if (vacancyDetail == null)
            {
                return null;
            }

            VacancyDetailViewModel vacancyDetailViewModel =
                _vacancyDetailMapper.Map<VacancyDetail, VacancyDetailViewModel>(vacancyDetail);
            return vacancyDetailViewModel;
        }
    }
}