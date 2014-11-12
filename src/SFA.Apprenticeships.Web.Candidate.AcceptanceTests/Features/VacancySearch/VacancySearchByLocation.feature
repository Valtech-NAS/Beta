Feature: Vacancy Search by Location
	In order to find a georgraphically suitable vacancy
	As a candidate
	I want to be able to find and refine vacancies by location

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@US449 @SmokeTests
Scenario: When searching by location the results are ordered by distance and order options do not contain best match
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value      |
		 | Location       | Birmingham |
		 | WithInDistance | 40 miles   |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see
        | Field                          | Rule         | Value                 |
        | SearchResultItemsCount         | Greater Than | 0                     |
        | SortOrderingDropDownItemsCount | Equals       | 2                     |
        | SortOrderingDropDownItemsText  | Equals       | Closing Date,Distance |
        | SortOrderingDropDown           | Equals       | Distance              |

@US449 @SmokeTests
Scenario: When searching by location the results are ordered by distance and distance is shown
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value      |
		 | Location       | Birmingham |
		 | WithInDistance | 40 miles   |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see SearchResults list contains
        | Field                | Rule   | Value |
        | DistanceDisplayed    | Equals | True  |
        | ClosingDateDisplayed | Equals | True  |
        | NationwideDisplayed  | Equals | False |

@USXXX @SmokeTests
Scenario: User enters location manually and sees a list of suggested locations
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value      |
		 | Location | Manchester |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                    | Rule         | Value                           |
        | LocationSuggestionsCount | Greater Than | 0                               |

@USXXX @SmokeTests
Scenario: User enters location manually and location defaults to first suggested location
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value  |
		 | Location | Covent |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field    | Rule   | Value                    |
        | Location | Equals | Coventry (West Midlands) |

@USXXX @SmokeTests
Scenario: User enters location manually then changes location manually and sees a list of suggested locations
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value      |
		 | Location | Manchester |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Birm  |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                    | Rule         | Value                      |
        | Location                 | Equals       | Birmingham (West Midlands) |
        | LocationSuggestionsCount | Greater Than | 0                          |

@USXXX @SmokeTests
Scenario: User enters location manually then selects from autocomplete and sees only their chosen location
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Leeds |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Leeds (West Yorkshire) |
	And I choose WrappedElement
	And I am on the VacancySearchPage page
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                        | Rule           | Value                  |
        | Location                     | Equals         | Leeds (West Yorkshire) |
        | LocationSuggestionsContainer | Does Not Exist |                        |

@USXXX @SmokeTests
Scenario: User enters location manually then selects from autocomplete then changes location manually and sees a list of suggested locations
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Leeds |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Leeds (West Yorkshire) |
	And I choose WrappedElement
	And I am on the VacancySearchPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Manchester  |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                    | Rule         | Value                           |
        | Location                 | Equals       | Manchester (Greater Manchester) |
        | LocationSuggestionsCount | Greater Than | 0                               |

@USXXX @SmokeTests
Scenario: User enters location manually then selects from autocomplete then refines twice and sees a list of suggestions for the final location search
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Leeds |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Leeds (West Yorkshire) |
	And I choose WrappedElement
	And I am on the VacancySearchPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Manchester  |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                    | Rule         | Value                           |
        | Location                 | Equals       | Manchester (Greater Manchester) |
        | LocationSuggestionsCount | Greater Than | 0                               |
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Birm  |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                    | Rule         | Value                      |
        | Location                 | Equals       | Birmingham (West Midlands) |
        | LocationSuggestionsCount | Greater Than | 0                          |

@USXXX @SmokeTests
Scenario: User enters location manually then selects from autocomplete then changes location manually then selects from autocomplete and sees a list of suggested locations
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Cov   |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                    | Rule         | Value |
        | Location                 | Starts With  | Cove  |
        | LocationSuggestionsCount | Greater Than | 0     |
	Then I clear the Location field
	When I enter data
		 | Field    | Value  |
		 | Location | Covent |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                    |
		| Text  | Equals | Coventry (West Midlands) |
	And I choose WrappedElement
	And I am on the VacancySearchResultPage page
	And I choose Search
	Then I am on the VacancySearchResultPage page
	And I see 
        | Field                        | Rule           | Value                    |
        | Location                     | Equals         | Coventry (West Midlands) |
        | LocationSuggestionsContainer | Does Not Exist |                          |

@SmokeTests
Scenario: Find apprenticeships by location and change ordering to closing date
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
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
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

@SmokeTests
Scenario: Find apprenticeships by location and change ordering to closing date and back again
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
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
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
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Distance |
	And I am on the VacancySearchResultPage page
	And I see
        | Field                     | Rule   | Value |
        | SearchResultItemsCount    | Equals | 5     |
        | ResultsAreInDistanceOrder | Equals | True  |
        #| ResultsAreInBestMatchScoreOrder | Equals | True  |
        #| ResultsAreInClosingDateOrder    | Equals | False |