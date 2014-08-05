@browser:disableJavaScript @vacancysearch @US58 @US230 @US258 
Feature: VacancySearchNoJS
	In order to confirm a candidate can search for vacancies
	as a candidate
	I want to be find relevant vacancies in my area

Scenario Outline: Search for apprenticeships in my area
	Given I am a candidate with preferences
	| Location | Distance |
	| Warwick  | 10 miles |
	And I enhance my search with the following '<search_keywords>'
	When I search for vacancies
	Then I expect to see search results
Examples: 
	| search_keywords | 
	|                 |
	| admin           |

Scenario: Search for apprenticeships - unspecified location
	Given I am a candidate with preferences
	| Location | Distance |
	| Warwick  | 10 miles |
	When I search for vacancies
	Then I expect to see a validation message
	| field_name | message                   |
	| location   | something to say about it |
