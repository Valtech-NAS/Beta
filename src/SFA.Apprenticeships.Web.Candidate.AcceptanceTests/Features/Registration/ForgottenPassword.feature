@US276
Feature: ResetPassword
	As a candidate who has forgotten my password
	I want to request to reset my password
	so that I can sign in to my account

Scenario: Password successful reset
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
	Then I wait 180 second for the ActivationPage page
	When I get the token for my newly created account
	And I enter data
		| Field          | Value             |
		| ActivationCode | {ActivationToken} |
	And I choose ActivateButton
	And I navigate to the ForgottenPasswordPage page
	When I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value               |
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
	And I choose ResetPasswordButton
	Then I am on the VacancySearchPage page
	And I see
		| Field              | Rule   | Value                                   |
		| SuccessMessageText | Equals | You've successfully reset your password |