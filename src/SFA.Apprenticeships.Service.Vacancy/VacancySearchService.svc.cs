namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
    using System.Linq;
    using Types;

    public class VacancySearchService : IVacancySearchService
    {
        public SearchResponse Search(SearchRequest request)
        {
            var results = new SearchProvider().Search(request);

            return new SearchResponse
            {
                Request = request,
                Results = results.ToArray()
            };
        }
    }
}
