@US496 @US449
Feature: Vacancy Search
	In order to find a vacancy apprenticeship quickly
	As a candidate
	I want to find a vacancy apprenticeship by location or keywords

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

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

Scenario: Find apprenticeships and test ordering without keywords
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
        #| ResultsAreInClosingDateOrder | Equals | False |
		# Need the ignore when not present added to specbind - KB talking to Dan Piessens
        #| ResultsAreInBestMatchScoreOrder | Equals | False |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	And I am on the VacancySearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | SearchResultItemsCount       | Equals | 5     |
        | ResultsAreInClosingDateOrder | Equals | True  |
		#| ResultsAreInDistanceOrder    | Equals | False | 
		# Need the ignore when not present added to specbind - KB talking to Dan Piessens
        #| ResultsAreInBestMatchScoreOrder | Equals | False |

Scenario: Find apprenticeships and test ordering with keywords
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Keywords       | Admin    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 5     |
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
        #| ResultsAreInDistanceOrder       | Equals | False |
        #| ResultsAreInClosingDateOrder    | Equals | False |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	And I am on the VacancySearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | SearchResultItemsCount       | Equals | 5     |
        | ResultsAreInClosingDateOrder | Equals | True  |
        #| ResultsAreInDistanceOrder       | Equals | False |
        #| ResultsAreInBestMatchScoreOrder | Equals | False |

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

Scenario: Search doesn't error when location doesn't exist
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value        |
		 | Location | KJHNSAKDFJHA |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
        | Field                | Rule           | Value |
        | SortOrderingDropDown | Does Not Exist |       |
        | NoResultsTitle       | Exists         |       |

Scenario: Search locaton autocomplete appears on both initial search page and search results page
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value    |
		 | Location | Coventry |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                    |
		| Text  | Equals | Coventry (West Midlands) |
	And I choose WrappedElement
	And I am on the VacancySearchPage page
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
	    | Field         | Rule   | Value |
	    | ClearLocation | Equals | True  |
	When I enter data
		 | Field    | Value  |
		 | Location | London |
	Then I wait for 5 seconds to see LocationAutoComplete

@US517
Scenario: Different results per page
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | London   |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 5     |
	When I enter data
		| Field                  | Value      |
		| ResultsPerPageDropDown | 10 per page |
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 10    |
	When I enter data
		| Field                  | Value      |
		| ResultsPerPageDropDown | 25 per page |
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 25    |
	When I enter data
		| Field                  | Value      |
		| ResultsPerPageDropDown | 50 per page |
	Then I am on the VacancySearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 50    |