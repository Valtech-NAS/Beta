Feature: SearchForApprenticeship
	In order to find a vacant apprenticeship quickly
	As a system user
	I want to find a vacant apprenticeship by entering the location

Scenario: Find vacant apprenticeship by location
	Given I navigated to the VacancySearchPage page
	And I was on the VacancySearchPage page
	When I enter data
		 | Field    | Value                    |
		 | Location | Coventry (West Midlands) |
	And I choose Search
	And I am on the VacancySearchResultPage page