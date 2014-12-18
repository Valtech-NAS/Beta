namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Application.Interfaces.Search;
    using Application.Interfaces.Vacancies;
    using ViewModels.VacancySearch;

    internal class TraineeshipSearchResultsResolver : ITypeConverter<SearchResults<TraineeshipSummaryResponse>, TraineeshipSearchResponseViewModel>
    {
        public TraineeshipSearchResponseViewModel Convert(ResolutionContext context)
        {
            var source = (SearchResults<TraineeshipSummaryResponse>)context.SourceValue;

            var viewModel = new TraineeshipSearchResponseViewModel
            {
                Vacancies = context.Engine.Map<IEnumerable<TraineeshipSummaryResponse>, IEnumerable<TraineeshipVacancySummaryViewModel>>(source.Results)
            };

            return viewModel;
        }
    }
}