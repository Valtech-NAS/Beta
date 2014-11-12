Feature: PrePopulate
	In order to speed up the application process
	As a candidate
	I want valid data I have previously entered to pre populate the application form

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@US461
Scenario: Pre-populate my personal and contact details
	Given I have registered a new candidate
	When I select the first vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
	And I see
		| Field                   | Rule   | Value              |
		| FullnameReadOnly        | Equals | Firstname Lastname |
		| EmailReadOnly           | Equals | {EmailToken}       |
		| DobReadOnly             | Equals | 01/01/2000         |
		| PhoneReadOnly           | Equals | 07970523193        |
		| AddressLine1ReadOnly    | Equals | Flat A             |
		| AddressLine2ReadOnly    | Equals | 6 Furlong Road     |
		| AddressLine3ReadOnly    | Equals | London             |
		| AddressLine4ReadOnly    | Equals | Islington          |
		| AddressPostcodeReadOnly | Equals | N7 8LS             |