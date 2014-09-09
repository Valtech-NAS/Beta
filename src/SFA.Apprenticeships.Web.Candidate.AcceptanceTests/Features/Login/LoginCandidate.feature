@US415
@US458
Feature: Login Candidate
	As a candidate,
	I want to sign in so that I can access my profile,
	apply for apprenticeships etc.

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@US415
Scenario: As a candidate all required fields are present
	Given I navigated to the LoginPage page
	When I am on the LoginPage page
	Then I wait to see EmailAddress
	And I wait to see Password
	And I see
		| Field             | Rule           | Value |
		| ValidationSummary | Does Not Exist |       |

@US415
Scenario: As a candidate I can login with a registered and activated email address and password
	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page

@US415
Scenario: As a candidate I must provide an email address and password
	Given I navigated to the LoginPage page
	When I am on the LoginPage page
	And I choose SignInButton
	And I am on the LoginPage page
	Then I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 2     |
@US415
Scenario: As a candidate I want to be redirected to the previous page when I login
	Given I registered an account and activated it
	And I navigated to the VacancySearchPage page
	When I choose SignInLink
	And I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the VacancySearchPage page

@US415
Scenario: As a candidate I cannot login with an invalid password
	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailAddressToken}    |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I wait to see ValidationSummary
	Then I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                          |
		| Text  | Equals | Please enter a valid email address or password |
		| Href  | Equals | #                                              |

@US458 @US444 @US456
Scenario: As a candidate I can login with a registered but unactivated account and am redirected to the activation page
	Given I registered an account but did not activate it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	And I am on the ActivationPage page
	And I enter data
		| Field          | Value                 |
		| ActivationCode | {ActivationCodeToken} |
	And I choose ActivateButton
	Then I am on the VacancySearchPage page

@US458 @US457
Scenario: Reset password after locking an account does not have to unlock the account
	Given I have registered a new candidate
	When I navigate to the LoginPage page
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailToken}           |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I am on the LoginPage page
	And I wait for 300 seconds to see ValidationSummary
	Then I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                          |
		| Text  | Equals | Please enter a valid email address or password |
		| Href  | Equals | #                                              |
	And I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| Password     | {InvalidPasswordToken} |
	#email is already in the form. We don't have to enter it another time
	And I choose SignInButton
	#should be removed when the button works properly
	And I choose SignInButton
	And I am on the LoginPage page
	And I wait for 300 seconds to see ValidationSummary
	Then I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                          |
		| Text  | Equals | Please enter a valid email address or password |
		| Href  | Equals | #                                              |
	And I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| Password     | {InvalidPasswordToken} |
	#email is already in the form. We don't have to enter it another time
	When I choose SignInButton
	#should be removed when the button works properly
	And I choose SignInButton
	Then I wait 180 second for the UnlockPage page
	And I get the account unlock code
	And I see
         | Field             | Rule   | Value        |
         | EmailAddressText  | Equals | {EmailToken} |
         | AccountUnlockCode | Exists |              |
	And the user login incorrect attempts should be three
	When I navigate to the ForgottenPasswordPage page
	Then I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	When I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And the user login incorrect attempts should be three
	And the account unlock code and date should be set
	And the password reset code and date should be set
	When I navigate to the LoginPage page
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
		| Password     | ?Password01! |
	And I choose SignInButton
	Then I am on the UnlockPage page
	When I navigate to the LoginPage page
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailToken}           |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	Then I am on the UnlockPage page
	And the account unlock code and date should be set
	And the password reset code and date should be set 
	When I choose ResendAccountUnlockCodeLink
	Then I get the same account unlock code
	When I navigate to the LoginPage page
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
		| Password     | ?Password01! |
	And I choose SignInButton
	Then I am on the UnlockPage page