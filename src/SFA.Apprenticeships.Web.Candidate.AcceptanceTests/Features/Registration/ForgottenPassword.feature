@US276
Feature: ResetPassword
	As a candidate who has forgotten my password
	I want to request to reset my password
	so that I can sign in to my account

Scenario: Password successful reset
	Given I registered an account and activated it
	And I navigated to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	When I get the token to reset the password
	And I navigate to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I get the same token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
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
		| Field        | Value               |
		| EmailAddress | {InvalidEmailToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I don't receive an email with the token to reset the password

Scenario: Reset password in an unactivated account
	Given I registered an account but did not activate it
	And I navigated to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
	And I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And I get the token to reset the password
	When I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | {NewPasswordToken}       |
	And I choose ResetPasswordButton
	Then I am on the VacancySearchPage page
	And I see
		| Field              | Rule   | Value                                   |
		| SuccessMessageText | Equals | You've successfully reset your password |