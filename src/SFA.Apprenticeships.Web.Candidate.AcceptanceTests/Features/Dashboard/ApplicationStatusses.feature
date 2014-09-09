Feature: ApplicationStatusses
	As a candidate who has already started an application for a vacancy 
	I want to see my application on my dashboard 
	And select to resume completing my application 
	And save any changes 
	so that I can edit my application and then save it for further edits at a future date .     

Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@US463
Scenario: As a candidate I want to see the applications in my dashbord grouped by state
	Given I have an empty dashboard
	And I add 4 applications in "Draft" state
	And I add 4 applications in "Successful" state
	And I add 4 applications in "Submitted" state
	And I add 4 applications in "Unsuccessful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                  | Rule   | Value |
		| DraftApplicationsCount | Equals | 4     |