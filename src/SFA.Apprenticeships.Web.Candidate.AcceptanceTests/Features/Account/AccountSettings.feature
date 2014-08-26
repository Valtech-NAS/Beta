@USS483
Feature: Account Settings - Personal Details
	As a candidate 
	I want to be able to make amendments to my first name, last name, date of birth, address and mobile phone number
	so that I can manage my personal details and make sure they are correct

Scenario: As a candidate I am on the settings page and all required fields are present and all validators show

	Given I registered an account and activated it
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page

	Given I navigated to the SettingsPage page
	When I am on the SettingsPage page

	And I wait to see Firstname
	And I wait to see Lastname
	And I wait to see Day
	And I wait to see Month
	And I wait to see Year

	And I choose UpdateDetailsButton

	And I wait to see ValidationSummary
	Then I see
        | Field                  | Rule   | Value |
        | ValidationSummaryCount | Equals | -1    |
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                   |
		| Text  | Equals | Please enter first name |
		| Href  | Equals | #firstname              |
	And I am on the SettingsPage page
	And I am on ValidationSummaryItems list item matching criteria
		| Field | Rule   | Value                  |
		| Text  | Equals | Please enter last name |
		| Href  | Equals | #lastname              |

Scenario: As a candidate I can change my name, date of birth and phone number
