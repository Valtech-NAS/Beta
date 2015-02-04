@US415
@US458
Feature: Login Candidate
	As a candidate,
	I want to sign in so that I can access my profile,
	apply for apprenticeships etc.

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@US415 @SmokeTests
Scenario: As a candidate all required fields are present
	Given I navigated to the LoginPage page
	When I am on the LoginPage page
	Then I wait to see EmailAddress
	And I wait to see Password
	And I see
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 0     |

@US415
Scenario: As a candidate I can login with a registered and activated email address and password
	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page

@US415 @SmokeTests
Scenario: As a candidate I must provide an email address and password
	Given I navigated to the LoginPage page
	When I am on the LoginPage page
	And I choose SignInButton
	And I am on the LoginPage page
	Then I see
		| Field                     | Rule   | Value |
		| ValidationFieldErrorCount | Equals | 2     |
@US415
Scenario: As a candidate I want to be redirected to the previous page when I login
	Given I registered an account and activated it
	And I navigated to the ApprenticeshipSearchPage page
	When I choose SignInLink
	And I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the ApprenticeshipSearchPage page

@US415
Scenario: As a candidate I cannot login with an invalid password
	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailToken}    |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I wait to see ValidationSummary
	Then I see
		| Field						   | Rule   | Value |
		| ValidationSummaryNoLinkCount | Equals | 1     |
	And I am on ValidationSummaryNoLinkItems list item matching criteria
		| Field | Rule   | Value                                          |
		| Text  | Equals | Please enter a valid email address or password |

@US458 @US444 @US456
Scenario: As a candidate I can login with a registered but unactivated account and am redirected to the activation page
	Given I registered an account but did not activate it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	And I am on the ActivationPage page
	And I enter data
		| Field          | Value                 |
		| ActivationCode | {ActivationCodeToken} |
	And I choose ActivateButton
	Then I am on the ApprenticeshipSearchPage page