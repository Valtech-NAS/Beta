Feature: SearchForApprenticeship
	In order to find a vacant apprenticeship quickly
	As a system user
	I want to find a vacant apprenticeship by entering the location

Scenario: Find vacant apprenticeship by location
	Given I navigated to the HomePage page
		And I navigated to the VacancySearchPage page
	When I enter data
		 | Field            | Value                    |
		 | Find by location | Coventry (West Midlands) |
		And I choose Search
		And I wait 10 second for the VacancySearchResultPage page
	Then I am on the VacancySearchResultPage page