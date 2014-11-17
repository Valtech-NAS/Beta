Feature: Vacancy Not found
	As an administrator I want to check that the error pages are configured correctly
	
Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@SmokeTests @ignore
Scenario: Add two numbers
	When I navigate to the details of the vacancy 9999999999
	Then I am on the VacancyNotFound page
