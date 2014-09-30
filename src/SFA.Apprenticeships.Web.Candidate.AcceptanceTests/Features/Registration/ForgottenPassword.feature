@US276
Feature: Forgotten Password
	As a candidate who has forgotten my password
	I want to request to reset my password
	so that I can sign in to my account

#TODO Refine background step to support cookie detection
Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

Scenario: Reset password successful
	Given I have registered a new candidate
	When I navigate to the ForgottenPasswordPage page
	Then I am on the ForgottenPasswordPage page
	When I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	When I get the token to reset the password
	And I navigate to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I get the same token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
		| ConfirmPassword   | {NewPasswordToken}       |
	And I choose ResetPasswordButton
	Then I am on the VacancySearchPage page
	And I see
		| Field              | Rule   | Value                                   |
		| SuccessMessageText | Equals | You've successfully reset your password |

Scenario: Reset password with an invalid email
	Given I registered an account and activated it
	And I navigated to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value                             |
		| EmailAddress | invalid.email.address@invalid.com |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I don't receive an email with the token to reset the password

Scenario: Reset password with a mismatching password
	Given I have registered a new candidate
	When I navigate to the ForgottenPasswordPage page
	Then I am on the ForgottenPasswordPage page
	When I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	
	Then I am on the ResetPasswordPage page
	When I get the token to reset the password
	And I navigate to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton

	Then I am on the ResetPasswordPage page
	And I get the same token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
		| ConfirmPassword   | !CannotPossiblyM4tch!    |
	And I choose ResetPasswordButton

	Then I am on the ResetPasswordPage page
	And I see
    | Field                  | Rule   | Value |
    | ValidationSummaryCount | Equals | 1     |
	
	And I am on the ResetPasswordPage page
	And I am on ValidationSummaryItems list item matching criteria
	| Field | Rule   | Value                             |
	| Text  | Equals | Sorry, your passwords don’t match |
	| Href  | Equals | #Password                         |

Scenario: Reset password in an unactivated account
	Given I navigated to the RegisterCandidatePage page
	When I have created a new email address
	And I enter data
		| Field           | Value         |
		| Firstname       | FirstnameTest |
		| Lastname        | LastnameTest  |
		| Phonenumber     | 07970523193   |
		| EmailAddress    | {EmailToken}  |
		| PostcodeSearch  | N7 8LS        |
		| Day             | 01            |
		| Month           | 01            |
		| Year            | 2000          |
		| Password        | ?Password01!  |
		| ConfirmPassword | ?Password01!  |
	And I choose HasAcceptedTermsAndConditions
	And I choose FindAddresses
	And I am on AddressDropdown list item matching criteria
		| Field        | Rule   | Value                  |
		| Text         | Equals | Flat A, 6 Furlong Road |
	And I choose WrappedElement
	And I am on the RegisterCandidatePage page
	And I choose CreateAccountButton
	Then I wait 120 second for the ActivationPage page
	When I navigate to the ForgottenPasswordPage page
	And I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I get the token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
		| ConfirmPassword   | {NewPasswordToken}       |
	And I choose ResetPasswordButton
	Then I am on the VacancySearchPage page
	And I see
		| Field              | Rule   | Value                                   |
		| SuccessMessageText | Equals | You've successfully reset your password |