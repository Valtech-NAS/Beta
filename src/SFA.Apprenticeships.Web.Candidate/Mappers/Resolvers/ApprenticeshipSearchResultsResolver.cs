namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using ViewModels.VacancySearch;

    internal class ApprenticeshipSearchResultsResolver : ITypeConverter<SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>, ApprenticeshipSearchResponseViewModel>
    {
        public ApprenticeshipSearchResponseViewModel Convert(ResolutionContext context)
        {
            var source = (SearchResults<ApprenticeshipSearchResponse, ApprenticeshipSearchParameters>)context.SourceValue;

            var viewModel = new ApprenticeshipSearchResponseViewModel
            {
                Vacancies = context.Engine.Map<IEnumerable<ApprenticeshipSearchResponse>, IEnumerable<ApprenticeshipVacancySummaryViewModel>>(source.Results)
            };

            return viewModel;
        }
    }
}