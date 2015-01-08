@US583
Feature: Traineeship Search Validation
	In order to find a traineeship vacancy quickly
	As a candidate
	I want invalid inputs to be highlighted before searching

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@SmokeTests
Scenario: Show validation error message when no location entered
	Given I navigated to the TraineeshipSearchPage page
	When I choose Search
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 1     |

@SmokeTests
Scenario: Show validation error message when single character location entered
	Given I navigated to the TraineeshipSearchPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | M     |
	When I choose Search
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 1     |

@SmokeTests
Scenario: Show validation error message when two character location entered that is not a partial postcode
	Given I navigated to the TraineeshipSearchPage page
	Then I clear the Location field
	When I enter data
		 | Field    | Value |
		 | Location | MA    |
	When I choose Search
	Then I am on the TraineeshipSearchResultPage page
	And I see
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 1     |
	And I am on ValidationFieldErrorItems list item matching criteria
		| Field | Rule   | Value                                               |
		| Text  | Equals | Location must be 3 or more characters or a postcode |