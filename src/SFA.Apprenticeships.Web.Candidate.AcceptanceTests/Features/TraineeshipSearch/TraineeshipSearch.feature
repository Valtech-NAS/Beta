@US583
Feature: Traineeship Search
	In order to find a traineeship vacancy quickly
	As a candidate
	I want to find a traineeship vacancy by location

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@SmokeTests
Scenario: Find traineeships and test ordering
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value  |
		 | Location | London |
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
Scenario: Find traineeships and change distance
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value    |
		 | Location | WC2E 9RZ |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field          | Rule   | Value    |
        | WithInDistance | Equals | 40 miles |
	When I enter data
		| Field          | Value   |
		| WithInDistance | 2 miles |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see
        | Field          | Rule   | Value   |
        | WithInDistance | Equals | 2 miles |

@SmokeTests
Scenario: Find traineeships and test paging
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value  |
		 | Location | London |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	And I wait to not see PreviousPage
	And I choose NextPage
	And I am on the TraineeshipSearchResultPage page
	And I wait to see PreviousPage
	Then I see
        | Field        | Rule     | Value |
        | NextPage     | Contains | 3 of  |
        | PreviousPage | Contains | 1 of  |
	When I choose NextPage
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field        | Rule     | Value |
        | NextPage     | Contains | 4 of  |
        | PreviousPage | Contains | 2 of  |
	When I choose PreviousPage
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field        | Rule     | Value |
        | NextPage     | Contains | 3 of  |
        | PreviousPage | Contains | 1 of  |
	When I choose PreviousPage
	And I am on the TraineeshipSearchResultPage page
	And I wait to not see PreviousPage
	Then I see
        | Field    | Rule     | Value |
        | NextPage | Contains | 2 of  |

@SmokeTests
Scenario: Search when no results are returned for location
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value  |
		 | Location | Dundee |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field                | Rule           | Value |
        | SortOrderingDropDown | Does Not Exist |       |
        | NoResultsTitle       | Exists         |       |

@SmokeTests
Scenario: Search doesn't error when location doesn't exist
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value        |
		 | Location | KJHNSAKDFJHA |
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
        | Field                | Rule           | Value |
        | SortOrderingDropDown | Does Not Exist |       |
        | NoResultsTitle       | Exists         |       |

@SmokeTests
Scenario: Search location autocomplete appears on both initial search page and search results page
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field    | Value    |
		 | Location | Coventry |
	Then I wait for 5 seconds to see LocationAutoComplete
	When I am on LocationAutoCompletItems list item matching criteria
		| Field | Rule   | Value                    |
		| Text  | Equals | Coventry (West Midlands) |
	And I choose WrappedElement
	And I am on the TraineeshipSearchPage page
	And I choose Search
	And I am on the TraineeshipSearchResultPage page
	Then I see
	    | Field         | Rule   | Value |
	    | ClearLocation | Equals | True  |
	When I enter data
		 | Field    | Value  |
		 | Location | London |
	Then I wait for 5 seconds to see LocationAutoComplete

@SmokeTests
Scenario: Different results per page
	Given I navigated to the TraineeshipSearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | London   |
	And I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 5     |
	When I enter data
		| Field                  | Value      |
		| ResultsPerPageDropDown | 10 per page |
	Then I am on the TraineeshipSearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 10    |
	When I enter data
		| Field                  | Value      |
		| ResultsPerPageDropDown | 25 per page |
	Then I am on the TraineeshipSearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 25    |
	When I enter data
		| Field                  | Value      |
		| ResultsPerPageDropDown | 50 per page |
	Then I am on the TraineeshipSearchResultPage page
	And I see
        | Field                  | Rule   | Value |
        | SearchResultItemsCount | Equals | 50    |
