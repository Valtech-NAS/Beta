@vacancydetail @US124 @US238 
Feature: VacancyDetails
	In order to confirm a candidate can view vacnacy full details.
	as a candidate
	I want to be read full vacancies details in my area

Scenario Outline: View apprenticeship details in my area
	Given I am a candidate searching for '<Location>' with a radius of '<Distance>'
	When I search for vacancies
	And I expect to see search results
	And I select the result returned from position '1'
	Then I expect to see a populated vacancy detail page
Examples: 
		| Location | Distance |
		| Warwick  | 10 miles |
		| London   | 10 miles |
		| Leeds    | 10 miles |
		| Bristol  | 10 miles |
		| CV1 2WT  | 10 miles |
		| N7 8LS   | 10 miles |
# Added these to test more data in early stages - reduce to one example once tested further