Feature: Vacancy Not found
	As an administrator I want to check that the error pages are configured correctly
	
Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@SmokeTests
Scenario: Vacancy not found should redirect to the error page
	Given I am in the right environment
	When I navigate to the details of the traineeship vacancy 42
	Then I am on the TraineeshipVacancyNotFound page
	And I see
        | Field   | Rule   | Value                           |
        | Heading | Equals | Traineeship no longer available |
