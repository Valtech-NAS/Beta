@vacancysearch @US58 @US230 @US258 
Feature: VacancySearch
	In order to confirm a candidate can search for vacancies
	as a candidate
	I want to be find relevant vacancies in my area

Scenario Outline: Search for apprenticeships in my area
	Given I am a candidate with preferences
	| Location | Within_Distance |
	| Coventry | 5 miles         |
	And I enhance my search with the following '<search_keywords>'
	When I search for vancancies
	Then I expect to see search results
Examples: 
	| search_keywords | 
	|                 |
	| admin           |

Scenario Outline: Search for apprenticeships - clear my criteria
	Given I am a candidate with preferences
	| Location | Within_Distance |
	| Coventry | 5 miles         |
	When I clear my search criteria
	Then I expect to see the search page
	And all search fields are reset
Examples: 
	| search_keywords          |
	| admin and other keywords |

Scenario: Search for apprenticeships - unspecified location
	Given I am a candidate with preferences
	| Location | Within_Distance |
	| Coventry | 5 miles         |
	When I search for vancancies
	Then I expect to see a validation message
	| field_name | message                   |
	| location   | something to say about it |
