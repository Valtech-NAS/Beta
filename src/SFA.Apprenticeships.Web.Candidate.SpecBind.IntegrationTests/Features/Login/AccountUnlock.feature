@Ignore
@US444
Feature: Account Unlock
	As the SFA I want to be able to lock a candidates account
	where they have made multiple attempts with an incorrect username/password combination
	so that I can be assured that the candidate is legitimate.

Scenario: Account is locked after 3 unsuccesful attempts
	Given I registered an account and activated it
	And I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailAddressToken}    |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I am on the LoginCandidatePage page
	And I wait to see ValidationSummary
	And I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	And I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailAddressToken}    |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I am on the LoginCandidatePage page
	And I wait to see ValidationSummary
	And I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	And I enter data
		| Field        | Value                  |
		| EmailAddress | {EmailAddressToken}    |
		| Password     | {InvalidPasswordToken} |
	And I choose SignInButton
	And I am on the LoginCandidatePage page
	And I wait to see ValidationSummary
	And I see
		| Field                  | Rule   | Value |
		| ValidationSummaryCount | Equals | 1     |
	Then I am on the AccountUnlockPage page

Scenario: Account can be unlocked with a valid, non-expired account unlock code

Scenario: Account can be unlocked and user is taken back to the page they originally requested

Scenario: Account unlock code can be resent

Scenario: Account unlock code is resent if it is expired
