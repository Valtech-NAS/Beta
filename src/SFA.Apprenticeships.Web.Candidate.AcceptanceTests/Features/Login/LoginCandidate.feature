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

Scenario: As a candidate all required fields are present
	Given I navigated to the LoginPage page
	When I am on the LoginPage page
	Then I wait to see EmailAddress
	And I wait to see Password
	And I see
		| Field             | Rule           | Value |
		| ValidationSummary | Does Not Exist |       |

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

Scenario: As a candidate I must provide an email address and password
	Given I navigated to the LoginPage page
	When I am on the LoginPage page
	And I choose SignInButton
	And I am on the LoginPage page
	Then I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 2     |

Scenario: As a candidate I want to be redirected to the previous page when I login
	Given I registered an account and activated it
	And I navigated to the RegisterCandidatePage page
	When I choose SignInLink
	And I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the RegisterCandidatePage page

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

@ignore
Scenario: Reset password after locking an account does not have to unlock the account
	Given I navigated to the RegisterCandidatePage page
	When I have created a new email address
	And I enter data
		| Field          | Value         |
		| Firstname      | FirstnameTest |
		| Lastname       | LastnameTest  |
		| Phonenumber    | 07970523193   |
		| EmailAddress   | {EmailToken}  |
		| PostcodeSearch | N7 8LS        |
		| Day            | 01            |
		| Month          | 01            |
		| Year           | 2000          |
		| Password       | ?Password01!  | 
	And I choose HasAcceptedTermsAndConditions
	And I choose FindAddresses
	And I am on AddressDropdown list item matching criteria
		| Field        | Rule   | Value                  |
		| Text         | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	Then I wait 240 second for the ActivationPage page
	When I get the token for my newly created account
	And I enter data
		| Field          | Value             |
		| ActivationCode | {ActivationToken} |
	And I choose ActivateButton
	Then I am on the VacancySearchPage page
	When I navigate to the LoginPage page
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailToken}           |
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
	And I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| Password     | {InvalidPasswordToken} |
	#email is already in the form. We don't have to enter it another time
	And I choose SignInButton
	And I wait to see ValidationSummary
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
	Then I am on the UnlockPage page
	And I see
         | Field             | Rule   | Value        |
         | EmailAddressText  | Equals | {EmailToken} |
         | AccountUnlockCode | Exists |              |
	And the user login incorrect attempts should be three
	When I navigate to the ForgottenPasswordPage page
	Then I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value               |
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
	Then I get the account unlock code
	And I wait to see ValidationSummary
	Then I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                                          |
		| Text  | Equals | Please enter a valid email address or password |
		| Href  | Equals | #                                              |
	And I am on the LoginPage page
	When I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
		| Password     | ?Password01! |
	And I choose SignInButton
	Then I am on the UnlockPage page