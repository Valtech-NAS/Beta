namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using ViewModels.VacancySearch;

    internal class TraineeshipSearchResultsResolver : ITypeConverter<SearchResults<TraineeshipSearchResponse, TraineeshipSearchParameters>, TraineeshipSearchResponseViewModel>
    {
        public TraineeshipSearchResponseViewModel Convert(ResolutionContext context)
        {
            var source = (SearchResults<TraineeshipSearchResponse, TraineeshipSearchParameters>)context.SourceValue;

            var viewModel = new TraineeshipSearchResponseViewModel
            {
                Vacancies = context.Engine.Map<IEnumerable<TraineeshipSearchResponse>, IEnumerable<TraineeshipVacancySummaryViewModel>>(source.Results)
            };

            return viewModel;
        }
    }
}