@vacancysearch @US58 @US230 @US258 
Feature: VacancySearchResults
	In order to confirm a candidate can search for vacancies
	as a candidate
	I want to be find relevant vacancies in my area

Scenario: View apprenticeships in my area
	Given I am a candidate with preferences
	| Location | Distance (miles) |
	| Warwick  | 10 miles         |
	And I have searched for vacancies
	When I see my first '10' search results
	Then I expect the search results to be sorted by 'sort-distance'

Scenario: View apprenticeships in my area - next page
	Given I am a candidate with preferences
	| Location               | Distance (miles) |
	| Warwick (Warwickshire) | 10 miles         |
	And I have searched for vacancies
	When I see my first '10' search results
	And I navigate to the next page of '10' results
	Then I expect to see the 'next' page of results