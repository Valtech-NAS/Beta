﻿@US444 @US276
Feature: Account Lock
	As the SFA I want to be able to lock a candidates account
	where they have made multiple attempts with an incorrect username/password combination
	so that I can be assured that the candidate is legitimate.

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

Scenario: Account is locked after three unsuccesful login attempts
	Given I made two unsuccessful login attempts
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	Then I am on the UnlockPage page
	And I see
         | Field             | Rule   | Value               |
         | EmailAddress      | Equals | {EmailAddressToken} |
         | AccountUnlockCode | Exists |                     |

Scenario: Account can be unlocked with a valid, non-expired account unlock code
	Given I locked my account
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the UnlockPage page
	And I enter data
		| Field             | Value                    |
		| AccountUnlockCode | {AccountUnlockCodeToken} |
	And I choose VerifyCodeButton
	Then I am on the LoginPage page
	And I see
         | Field              | Rule   | Value |
         | SuccessMessageText | Exists |       |

Scenario: Account unlock code can be resent
	Given I locked my account
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the UnlockPage page
	And I enter data
		| Field             | Value                    |
		| AccountUnlockCode | {AccountUnlockCodeToken} |
	And I choose ResendAccountUnlockCodeLink
	Then I am on the UnlockPage page
	And I see
         | Field          | Rule   | Value |
         | ResentCodeText | Exists |       |

Scenario: Account unlock code is renewed if it has expired
	Given I locked my account and my account unlock code has expired
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the UnlockPage page
	And I enter data
		| Field             | Value                    |
		| AccountUnlockCode | {AccountUnlockCodeToken} |
	And I choose VerifyCodeButton
	Then I am on the UnlockPage page
	And my account unlock code has been renewed

Scenario: Account unlock code is renewed before being resent if it has expired
	Given I locked my account and my account unlock code has expired
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the UnlockPage page
	And I choose ResendAccountUnlockCodeLink
	Then I am on the UnlockPage page
	And I see
         | Field                       | Rule   | Value |
         | ResentCodeText | Exists |       |
	And my account unlock code has been renewed

@US491 @AC8
Scenario: Reset password after locking an account unlocks the account
	Given I have registered a new candidate
	When I choose SignoutLink
	Then I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailToken}           |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I am on the LoginPage page
	When I enter data
		| Field        | Value                  |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	Then I wait 180 second for the UnlockPage page
	And I get the account unlock code
	And I see
         | Field             | Rule   | Value        |
         | EmailAddress      | Equals | {EmailToken} |
         | AccountUnlockCode | Exists |              |
	And the user login incorrect attempts should be 3
	When I navigate to the ForgottenPasswordPage page
	Then I am on the ForgottenPasswordPage page
	And I enter data
		| Field        | Value        |
		| EmailAddress | {EmailToken} |
	When I choose SendCodeButton
	Then I am on the ResetPasswordPage page
	And the user login incorrect attempts should be 3
	And the account unlock code and date should be set
	And the password reset code and date should be set	
	Then I get the password reset code
	And I enter data
		| Field             | Value                    |
		| PasswordResetCode | {PasswordResetCodeToken} |
		| Password          | ?NewPassword02!          |
		| ConfirmPassword   | ?NewPassword02!          |
	When I choose ResetPasswordButton
	Then I am on the HomePage page
	And the user login incorrect attempts should be 0
	And the account unlock code and date should not be set
	And the password reset code and date should not be set