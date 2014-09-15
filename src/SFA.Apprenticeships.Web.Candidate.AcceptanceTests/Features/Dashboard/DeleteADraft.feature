Feature: DeleteADraft
	As an candidate who has one or more active draft applications 
	I want to be able to delete draft applications 
	so that I can remove any applications that I no longer need.

Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@US464
Scenario: As an candidate I want to be able to delete draft applications 
	Given I have an empty dashboard
	And I add 2 applications in "Draft" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	When I choose DeleteDraftLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                         | Rule   | Value |
		| DraftApplicationsCount        | Equals | 1     |
