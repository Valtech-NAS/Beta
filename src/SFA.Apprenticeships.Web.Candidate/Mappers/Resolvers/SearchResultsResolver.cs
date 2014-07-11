namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using ViewModels.VacancySearch;

    internal class SearchResultsResolver : ITypeConverter<SearchResults<VacancySummaryResponse>, VacancySearchResponseViewModel>
    {
        public VacancySearchResponseViewModel Convert(ResolutionContext context)
        {
            var source = (SearchResults<VacancySummaryResponse>)context.SourceValue;

            var viewModel = new VacancySearchResponseViewModel
            {
                TotalHits = source.Total,
                Vacancies = context.Engine.Map<IEnumerable<VacancySummaryResponse>, IEnumerable<VacancySummaryViewModel>>(source.Results)
            };

            return viewModel;
        }
    }
}