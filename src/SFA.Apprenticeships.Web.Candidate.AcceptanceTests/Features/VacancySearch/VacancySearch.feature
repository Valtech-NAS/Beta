@US496 @US449
Feature: Vacancy Search
	In order to find a vacancy apprenticeship quickly
	As a candidate
	I want to find a vacancy apprenticeship by location or keywords

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@SmokeTests
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

@SmokeTests
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
        | Field                           | Rule   | Value |
        | SearchResultItemsCount          | Equals | 5     |
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
        #| ResultsAreInDistanceOrder       | Equals | False |
        #| ResultsAreInClosingDateOrder    | Equals | False |

@SmokeTests
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

@SmokeTests
Scenario: Search when no results are returned for location
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value  |
		 | Keywords | Chef   |
		 | Location | Dundee |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
        | Field                | Rule           | Value |
        | SortOrderingDropDown | Does Not Exist |       |
        | NoResultsTitle       | Exists         |       |

@SmokeTests
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

@SmokeTests
Scenario: Search location autocomplete appears on both initial search page and search results page
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

@US517 @SmokeTests
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

@US528 @SmokeTests
Scenario: Return to search results link appears if arriving from search results page
	Given I select the "first" vacancy in location "N7 8LS" that can apply by this website
	When I am on the VacancyDetailsPage page
	Then I see
		| Field                     | Rule   | Value |
		| ReturnToSearchResultsLink | Exists |       |

@US528 @SmokeTests @ignore
Scenario: Return to find apprenticeship link appears if not arriving from search results page
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	And I am on the VacancySearchResultPage page
	And I set token VacancyId with the value of FirstVacancyId
	And I navigate to the HomePage page
	And I navigate to the VacancyDetailsPage page with parameters
		| VacancyId   |
		| {VacancyId} |
	Then I see
		| Field                  | Rule   | Value |
		| FindApprenticeshipLink | Exists |       |