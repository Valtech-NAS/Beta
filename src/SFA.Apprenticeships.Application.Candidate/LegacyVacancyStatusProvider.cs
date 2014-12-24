namespace SFA.Apprenticeships.Application.Candidate
{
    using System;
    using Domain.Entities.Vacancies;

    public class LegacyVacancyStatusProvider : IVacancyStatusProvider
    {
        public VacancyStatuses GetVacancyStatus(int vacancyId)
        {
            //todo: return the status for a given vacancy. 
            // first queries the search index (currently if not present then not a live vacancy)
            // then try the cached vacancy details provider (which in turn will call the NAS gateway)
            throw new NotImplementedException();
        }
    }
}
