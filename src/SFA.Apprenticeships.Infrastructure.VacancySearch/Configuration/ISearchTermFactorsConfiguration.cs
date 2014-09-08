namespace SFA.Apprenticeships.Infrastructure.VacancySearch.Configuration
{
    public interface ISearchTermFactorsConfiguration
    {
        string FieldName { get; set; }

        double? Boost { get; set; }

        int? Fuzziness { get; set; }

        int? FuzzyPrefix { get; set; }

        bool MatchAllKeywords { get; set; }

        int? PhraseProximity { get; set; }

        string MinimumMatch { get; set; }
    }
}
