@US496 @US449
Feature: Vacancy Search Validation
	In order to find a vacancy apprenticeship quickly
	As a candidate
	I want invalid inputs to be highlighted before searching

Background: 
	Given I navigated to the VacancySearchPage page
	And I am logged out
	And I navigated to the VacancySearchPage page
	Then I am on the VacancySearchPage page

@SmokeTests
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
