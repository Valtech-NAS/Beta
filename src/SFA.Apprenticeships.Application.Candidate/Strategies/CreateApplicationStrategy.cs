namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;
    using Interfaces.Vacancies;

    public class CreateApplicationStrategy : ICreateApplicationStrategy
    {
        private readonly IVacancyDataProvider _vacancyDataProvider;

        public CreateApplicationStrategy(
            IVacancyDataProvider vacancyDataProvider)
        {
            _vacancyDataProvider = vacancyDataProvider;
        }

        public ApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            // TODO: get vacancy so can check current status and include some snapshot detail in the application (VacancySummary)
            // IVacancyDataProvider.GetVacancyDetails(legacyVacancyId)
            var vacancyDetails = _vacancyDataProvider.GetVacancyDetails(vacancyId);

            // TODO: existing application
            // if exists and status = submitting/submitted (or any other "post submission" state) then cannot create new one
            // if exists and vacancy has expired then return it so user can view it
            // if exists and status = draft then return it so user can continue with the application form

            // TODO: new application
            // if vacancy has expired then throw ex as candidate cannot apply for it
            // create a new application entity, get template info, set vacancy summary info, save to app repo as "draft", return it

            throw new NotImplementedException();
        }
    }
}
