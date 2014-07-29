namespace SFA.Apprenticeships.Application.Candidate.Strategies
{
    using System;
    using Domain.Entities.Applications;

    public class LegacyCreateApplicationStrategy : ICreateApplicationStrategy
    {
        public ApplicationDetail CreateApplication(Guid candidateId, int vacancyId)
        {
            //todo: get vacancy so can check current status and include some snapshot detail in the application (VacancySummary)
            //IVacancyDataProvider.GetVacancyDetails(legacyVacancyId)

            //todo: existing application
            // if exists and status = submitting/submitted (or any other "post submission" state) then cannot create new one
            // if exists and vacancy has expired then return it so user can view it
            // if exists and status = draft then return it so user can continue with the application form

            //todo: new application
            // if vacancy has expired then throw ex as candidate cannot apply for it
            // create a new application entity, get template info, set vacancy summary info, save to app repo as "draft", return it

            throw new NotImplementedException();
        }
    }
}
