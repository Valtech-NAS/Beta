namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.Linq;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using Types;

    public class SearchProvider
    {
        public VacancySummaryResponse[] Search(SearchRequest request)
        {
            //todo: 
            // 1. map request parameter values to search request
            //    will be passed as string... map to enum and parse values (bool, double, etc.)
            //    if any fail, return "bad request" fault with name of failed parameters
            //
            // 2. invoke search component (using NEST) passing in the parsed parameters and values
            //    map search results to DTOs (incl. score)
            //
            // 3. return DTOs along with original request (for correlation in test tool)

            var results = Enumerable.Range(1, 10).Select(i => new VacancySummaryResponse
            {
                Id = i,
                Title = "Title #" + i,
                Description = "Vacancy description #" + i,
                EmployerName = "Employer name #" + i,
                ClosingDate = DateTime.UtcNow.AddDays(i),
                Score = 1.0
            });

            return results.ToArray();
        }
    }
}
