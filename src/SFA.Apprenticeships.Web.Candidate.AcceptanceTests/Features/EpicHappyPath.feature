@ignore
Feature: EpicHappyPath
	In order to test the epic happy path
	As a candidate
	I want to be able to search, view, register, login, save, apply, view, resume and dismiss applications

Scenario: Epic happy path
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field          | Value    |
		 | Location       | Coventry |
		 | WithInDistance | 40 miles |
	And I choose Search
	Then I am on the VacancySearchResultPage page
	When I choose FirstVacancyLink
	And I am on the DetailsPage page
	And I choose Apply
	Then I am on the LoginPage page
	When I choose CreateAccountLink
	Then I am on the RegisterPage page



