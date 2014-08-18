Feature: Vacancy Search
	In order to find a vacancy apprenticeship quickly
	As a candidate
	I want to find a vacancy apprenticeship by location or keywords


Scenario: Show validation error message when no location entered
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | WithInDistance | 40 miles |
	And I choose Search
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 1     |

Scenario: Find apprenticeships and test ordering
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
        | Field                     | Rule   | Value |
        | SearchResultItemsCount    | Equals | 5     |
        | ResultsAreInDistanceOrder | Equals | True  |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	And I am on the VacancySearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | SearchResultItemsCount       | Equals | 5     |
        | ResultsAreInClosingDateOrder | Equals | True  |

Scenario: Find apprenticeships and test paging
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	And I am on the VacancySearchResultPage page
	And I wait to not see PreviousPage
	And I choose NextPage
	And I am on the VacancySearchResultPage page
	And I wait to see PreviousPage
	Then I see
        | Field        | Rule     | Value |
        | NextPage     | Contains | 3 of  |
        | PreviousPage | Contains | 1 of  |
	When I choose NextPage
	And I am on the VacancySearchResultPage page
	Then I see
        | Field        | Rule     | Value |
        | NextPage     | Contains | 4 of  |
        | PreviousPage | Contains | 2 of  |
	When I choose PreviousPage
	And I am on the VacancySearchResultPage page
	Then I see
        | Field        | Rule     | Value |
        | NextPage     | Contains | 3 of  |
        | PreviousPage | Contains | 1 of  |
	When I choose PreviousPage
	And I am on the VacancySearchResultPage page
	And I wait to not see PreviousPage
	Then I see
        | Field    | Rule     | Value |
        | NextPage | Contains | 2 of  |

Scenario: Search when no results are returned for location
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value  |
		 | Location | Dundee |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
        | Field                | Rule           | Value |
        | SortOrderingDropDown | Does Not Exist |       |
        | NoResultsTitle       | Exists         |       |