Feature: ArchiveApplications
	As a candidate,
	I want to be able to view the status of my application(s)
	so that I can track the progress of my application(s) 

Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@US154
Scenario: As an candidate I want to be able to archive succesful applications 
	Given I have an empty dashboard
	And I add 2 applications in "Successful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	When I choose ArchiveSuccessfulLink 
	And I see an alert box and select Yes
	Then I am on the MyApplicationsPage page
	And I see
		| Field                         | Rule   | Value |
		| SuccessfulApplicationsCount   | Equals | 1     |

@US154
Scenario: As an candidate I want to be able to archive unsuccesful applications 
	Given I have an empty dashboard
	And I add 2 applications in "Unsuccessful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	When I choose ArchiveUnsuccessfulLink 
	And I see an alert box and select Yes
	Then I am on the MyApplicationsPage page
	And I see
		| Field                         | Rule   | Value |
		| UnsuccessfulApplicationsCount | Equals | 1     |
