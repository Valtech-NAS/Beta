@US444
Feature: Account Lock
	As the SFA I want to be able to lock a candidates account
	where they have made multiple attempts with an incorrect username/password combination
	so that I can be assured that the candidate is legitimate.

Scenario: Account is locked after three unsuccesful login attempts
	Given I made two unsuccessful login attempts
	And I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	Then I am on the AccountUnlockPage page
	And I see
         | Field             | Rule   | Value               |
         | EmailAddressText  | Equals | {EmailAddressToken} |
         | AccountUnlockCode | Exists |                     |

Scenario: Account can be unlocked with a valid, non-expired account unlock code
	Given I locked my account
	And I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the AccountUnlockPage page
	And I enter data
		| Field             | Value                    |
		| AccountUnlockCode | {AccountUnlockCodeToken} |
	And I choose VerifyCodeButton
	Then I am on the VacancySearchPage page

Scenario: Account unlock code can be resent
	Given I locked my account
	And I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the AccountUnlockPage page
	And I enter data
		| Field             | Value                    |
		| AccountUnlockCode | {AccountUnlockCodeToken} |
	And I choose ResendAccountUnlockCodeLink
	Then I am on the AccountUnlockPage page
	And I see
         | Field    | Rule   | Value |
         | Preamble | Exists |       |

Scenario: Account unlock code is renewed if it has expired
	Given I locked my account and my account unlock code has expired
	And I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the AccountUnlockPage page
	And I enter data
		| Field             | Value                    |
		| AccountUnlockCode | {AccountUnlockCodeToken} |
	And I choose VerifyCodeButton
	Then I am on the AccountUnlockPage page
	And my account unlock code has been renewed

Scenario: Account unlock code is renewed before being resent if it has expired
	Given I locked my account and my account unlock code has expired
	And I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | S3cret!             |
	And I choose SignInButton
	And I am on the AccountUnlockPage page
	And I choose ResendAccountUnlockCodeLink
	Then I am on the AccountUnlockPage page
	And I see
         | Field    | Rule   | Value |
         | Preamble | Exists |       |
	And my account unlock code has been renewed