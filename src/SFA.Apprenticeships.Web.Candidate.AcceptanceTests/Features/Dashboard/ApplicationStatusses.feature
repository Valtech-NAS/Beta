Feature: ApplicationStatusses
	As a candidate who has already started an application for a vacancy 
	I want to see my application on my dashboard 
	And select to resume completing my application 
	And save any changes 
	so that I can edit my application and then save it for further edits at a future date .     

Background: 
	Given I navigated to the VacancySearchPage page
	And I am logged out
	And I navigated to the VacancySearchPage page
	Then I am on the VacancySearchPage page

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
		| Field                         | Rule   | Value |
		| DraftApplicationsCount        | Equals | 4     |
		| SubmittedApplicationsCount    | Equals | 4     |
		| SuccessfulApplicationsCount   | Equals | 4     |
		| UnsuccessfulApplicationsCount | Equals | 4     |

@US366
Scenario: As a candidate I want to see the traineeships prompt if I have three or more unsuccessful applications
Given I have an empty dashboard
	And I add 3 applications in "Unsuccessful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                       | Rule   | Value |
		| TraineeshipsPromptDisplayed | Equals | True  |
	When I choose TraineeshipOverviewLink
	Then I am on the TraineeshipOverviewPage page

@US366
Scenario: As a candidate I dont want to see the traineeships prompt if I have less than three unsuccessful applications
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
	And I see
		| Field              | Rule           | Value |
		| TraineeshipsPrompt | Does Not Exist |       |
