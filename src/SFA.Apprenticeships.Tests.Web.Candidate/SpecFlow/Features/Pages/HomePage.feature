Feature: HomePage
	In order to check the site is working
	As a user
	I want to be told the page has loaded

@mytag
Scenario: Home page loads
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
