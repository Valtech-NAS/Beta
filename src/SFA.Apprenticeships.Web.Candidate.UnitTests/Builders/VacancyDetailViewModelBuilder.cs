namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Builders
{
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;

    public class VacancyDetailViewModelBuilder
    {
        private VacancyStatuses _vacancyStatus = VacancyStatuses.Live;

        public VacancyDetailViewModelBuilder WithVacancyStatus(VacancyStatuses vacancyStatus)
        {
            _vacancyStatus = vacancyStatus;
            return this;
        }

        public VacancyDetailViewModel Build()
        {
            var model = new VacancyDetailViewModel
            {
                VacancyStatus = _vacancyStatus
            };

            return model;
        }
    }
}