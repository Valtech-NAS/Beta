Feature: Vacancy Not found
	As an administrator I want to check that the error pages are configured correctly
	
Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@SmokeTests
Scenario: Vacancy not found should redirect to the error page
	Given I am in the right environment
	When I navigate to the details of the apprenticeship vacancy 42
	Then I am on the ApprenticeshipVacancyNotFound page
	And I see
        | Field   | Rule   | Value                              |
        | Heading | Equals | Apprenticeship no longer available |
