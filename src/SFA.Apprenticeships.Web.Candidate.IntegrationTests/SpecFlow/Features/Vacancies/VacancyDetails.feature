@vacancydetail @US124 @US238 
Feature: VacancyDetails
	In order to confirm a candidate can view vacnacy full details.
	as a candidate
	I want to be read full vacancies details in my area

@mytag
Scenario: View apprenticeship details in my area
	Given I am a candidate with preferences
	| Location | Distance |
	| Warwick  | 10 miles |
	When I search for vacancies
	Then I expect to see search results
	Then I select the result from position '1'
