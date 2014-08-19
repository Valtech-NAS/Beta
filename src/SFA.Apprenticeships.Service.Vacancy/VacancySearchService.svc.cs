namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using Types;

    public class VacancySearchService : IVacancySearchService
    {
        public SearchResponse Search(SearchRequest request)
        {
            //todo: 
            // 1. map request parameter values to search request
            // 2. invoke search using parameters
            // 3. map search results to DTOs
            // 4. return with original request

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
