﻿Feature: Traineeship applications dashboard
	As a candidate who has applied for one or more traineeships
	I want to see my traineeship applications on my dashboard 

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@US586
Scenario: I have applied for two traineeships
	Given I have an empty dashboard
	And I applied for 2 traineeships
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field             | Rule   | Value |
		| TraineeshipsCount | Equals | -1    |
