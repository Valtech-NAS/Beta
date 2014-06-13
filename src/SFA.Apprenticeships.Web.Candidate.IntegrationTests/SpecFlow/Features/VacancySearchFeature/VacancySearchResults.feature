@vacancysearch @US58 @US230 @US258 
Feature: VacancySearchResults
	In order to confirm a candidate can search for vacancies
	as a candidate
	I want to be find relevant vacancies in my area

Scenario: View apprenticeships in my area
	Given I am a candidate with preferences
		| Location | Distance |
		| Warwick  | 10 miles |
	And I have searched for vacancies
	When I see my first '10' search results
	Then I expect the search results to be sorted by 'sort-distance'

Scenario: View apprenticeships in my area - next page
	Given I am a candidate with preferences
		| Location | Distance |
		| Warwick  | 10 miles |
	And I have searched for vacancies
	When I see my first '10' search results
		* I have paged through the next '1' pages
	Then I expect to see the results for page '2'

Scenario: View apprenticeships in my area - prev page
	Given I am a candidate with preferences
		| Location | Distance |
		| Warwick  | 10 miles |
	And I have searched for vacancies
		* I see my first '10' search results
		* I have paged through the next '3' pages
	When I have paged through the previous '2' pages
	Then I expect to see the results for page '2'