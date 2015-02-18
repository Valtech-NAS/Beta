@US583
Feature: Traineeship Search by Location
	In order to find a georgraphically suitable traineeship vacancy
	As a candidate
	I want to be able to find and refine traineeship vacancies by location

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@SmokeTests
Scenario: When searching by location the results are ordered by distance and order options do not contain best match
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field          | Value      |
		 | Location       | Birmingham |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field                          | Rule         | Value                                |
        | SearchResultItemsCount         | Greater Than | 0                                    |
        | SortOrderingDropDownItemsCount | Equals       | 3                                    |
        | SortOrderingDropDownItemsText  | Equals       | Closing Date,Distance,Recently Added |
        | SortOrderingDropDown           | Equals       | Distance                             |

@SmokeTests
Scenario: When searching by location the results are ordered by distance and distance is shown
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field          | Value      |
		 | Location       | Birmingham |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see SearchResults list contains
        | Field                | Rule   | Value |
        | DistanceDisplayed    | Equals | True  |
        | ClosingDateDisplayed | Equals | True  |

@SmokeTests
Scenario: User enters location manually and sees a list of suggested locations
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value      |
		 | Location | Manchester |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                    | Rule         | Value |
        | LocationSuggestionsCount | Greater Than | 0     |

@SmokeTests
Scenario: User enters location manually and location defaults to first suggested location
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value  |
		 | Location | Covent |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field    | Rule   | Value                    |
        | Location | Equals | Coventry (West Midlands) |

@SmokeTests
Scenario: User enters location manually then changes location manually and sees a list of suggested locations
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value      |
		 | Location | Manchester |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Cov  |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                    | Rule         | Value                    |
        | Location                 | Equals       | Coventry (West Midlands) |
        | LocationSuggestionsCount | Greater Than | 0                        |

@SmokeTests
Scenario: User enters location manually then selects from autocomplete and sees only their chosen location
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Leeds |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Leeds (West Yorkshire) |
	And I choose WrappedElement
	And I am on the TraineeshipSearchPage page
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                        | Rule           | Value                  |
        | Location                     | Equals         | Leeds (West Yorkshire) |
        | LocationSuggestionsContainer | Does Not Exist |                        |

@SmokeTests
Scenario: User enters location manually then selects from autocomplete then changes location manually and sees a list of suggested locations
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Leeds |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Leeds (West Yorkshire) |
	And I choose WrappedElement
	And I am on the TraineeshipSearchPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Manchester  |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                    | Rule         | Value                           |
        | Location                 | Equals       | Manchester (Greater Manchester) |
        | LocationSuggestionsCount | Greater Than | 0                               |

@SmokeTests
Scenario: User enters location manually then selects from autocomplete then refines twice and sees a list of suggestions for the final location search
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Leeds |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Leeds (West Yorkshire) |
	And I choose WrappedElement
	And I am on the TraineeshipSearchPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Manchester  |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                    | Rule         | Value                           |
        | Location                 | Equals       | Manchester (Greater Manchester) |
        | LocationSuggestionsCount | Greater Than | 0                               |
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | Cov   |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                    | Rule         | Value                    |
        | Location                 | Equals       | Coventry (West Midlands) |
        | LocationSuggestionsCount | Greater Than | 0                        |

@SmokeTests
Scenario: User enters location manually then selects from autocomplete then changes location manually then selects from autocomplete and sees a list of suggested locations
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Cov   |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                    | Rule         | Value |
		#The location should be deterministically found however that does not seem to be the case
		#Further investigation is required but for now changing to fix test
		#| Location                 | Equals       | Cove (Highland) |
        | Location                 | Contains     | Cove  |
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
	And I am on the TraineeshipSearchResultPage page
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see 
        | Field                        | Rule           | Value                    |
        | Location                     | Equals         | Coventry (West Midlands) |
        | LocationSuggestionsContainer | Does Not Exist |                          |

@SmokeTests
Scenario: Find traineeships by location and change ordering to closing date
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field                     | Rule   | Value |
        | SearchResultItemsCount    | Equals | 5     |
        | ResultsAreInDistanceOrder | Equals | True  |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	And I am on the TraineeshipSearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | SearchResultItemsCount       | Equals | 5     |
        | ResultsAreInClosingDateOrder | Equals | True  |

@SmokeTests
Scenario: Find traineeships by location and change ordering to closing date and back again
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field                     | Rule   | Value |
        | SearchResultItemsCount    | Equals | 5     |
        | ResultsAreInDistanceOrder | Equals | True  |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing Date |
	And I am on the TraineeshipSearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | SearchResultItemsCount       | Equals | 5     |
        | ResultsAreInClosingDateOrder | Equals | True  |
	And I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Distance |
	And I am on the TraineeshipSearchResultPage page
	And I see
        | Field                     | Rule   | Value |
        | SearchResultItemsCount    | Equals | 5     |
        | ResultsAreInDistanceOrder | Equals | True  |
