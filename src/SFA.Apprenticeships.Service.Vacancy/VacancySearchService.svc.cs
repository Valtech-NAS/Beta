namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using Types;

    public class VacancySearchService : IVacancySearchService
    {
        public SearchResponse Search(SearchRequest request)
        {
            //todo: invoke search, map results and return...

            return new SearchResponse
            {
                Request = request,
                Results = new[]
                {
                    new VacancySummary {Id = 1},
                    new VacancySummary {Id = 2},
                    new VacancySummary {Id = 3},
                    new VacancySummary {Id = 4},
                    new VacancySummary {Id = 5}
                }
            };
        }
    }
}
