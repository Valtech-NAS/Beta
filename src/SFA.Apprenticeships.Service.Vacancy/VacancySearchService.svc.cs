namespace SFA.Apprenticeships.Service.Vacancy
{
    using Types;

    public class VacancySearchService : IVacancySearchService
    {
        public SearchResponse Search(SearchRequest request)
        {
            var results = new SearchProvider().Search(request);

            return new SearchResponse
            {
                Request = request,
                SearchResults = results
            };
        }
    }
}
