@ignore
Feature: SearchForApprenticeship
	In order to find a vacancy apprenticeship quickly
	As a system user
	I want to find a vacancy apprenticeship by location or keywords

Scenario: Find apprenticeships by location
	Given I navigated to the VacancySearchPage page
	When I enter data
		 | Field    | Value    |
		 | Location | Coventry |
	And I choose Search
	And I am on the VacancySearchResultPage page
	Then I see SearchResults list Exists
	    | Field            | Rule   | Value |
	    | Title            | Exists |       |
	    | Subtitle         | Exists |       |
	    | ShortDescription | Exists |       |