@US496 @US449
Feature: Vacancy Search Validation
	In order to find a apprenticeship vacancy quickly
	As a candidate
	I want invalid inputs to be highlighted before searching

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@SmokeTests
Scenario: Show validation error message when no location entered
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field          | Value    |
		 | WithInDistance | 40 miles |
	And I choose Search
	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | 1     |
