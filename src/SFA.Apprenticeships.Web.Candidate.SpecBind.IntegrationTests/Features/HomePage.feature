Feature: HomePage
	In order to check that I am on the home page
	As a candidate user
	I want to check the header of the page

Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@pageload
Scenario: Home Page Test Scenario
	Then I wait to see Header

@pagenavigation
Scenario: Vacancy Search Navigation Test
	When I choose VacancySearchLink
	Then I wait for the VacancySearchPage page