Feature: Vacancy Not found
	As an administrator I want to check that the error pages are configured correctly
	
Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@SmokeTests
Scenario: Vacancy not found should redirect to the error page
	Given I am in the right environment
	When I navigate to the details of the vacancy 19999999
	Then I am on the VacancyNotFound page
