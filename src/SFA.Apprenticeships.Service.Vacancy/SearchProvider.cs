namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Types;

    public class SearchProvider
    {
        public IEnumerable<VacancySummary> Search(SearchRequest request)
        {
            //todo: 
            // 1. map request parameter values to search request
            //    will be passed as string... map to enum and parse values (bool, double, etc.)
            //    if any fail, return "bad request" fault with name of failed parameters
            //
            // 2. invoke search component (uses NEST) passing in the parsed parameters
            //    map search results to DTOs (incl. score)
            //
            // 3. return DTOs along with original request (for correlation in test tool)

            return Enumerable.Range(1, 10).Select(i => new VacancySummary
            {
                VacancyId = i,
                Title = "Title #" + i,
                Description = "Vacancy description #" + i,
                EmployerName = "Employer name #" + i,
                ClosingDate = DateTime.UtcNow.AddDays(i),
                Score = 1.0
            });
        }
    }
}
