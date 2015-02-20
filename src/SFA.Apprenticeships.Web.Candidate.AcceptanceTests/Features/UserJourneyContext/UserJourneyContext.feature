@US689
Feature: Branding
	In order to make navigation consistent
	I want the header to show the wording corresponding to the user journey I am in

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

Scenario: Change branding
	Given I registered an account and activated it
	And I navigated to the ApprenticeshipSearchPage page
	Then I see
		| Field       | Rule     | Value          |
		| GlobalTitle | Contains | apprenticeship |
	When I choose SignInLink
	And I am on the LoginPage page
	Then I see
		| Field       | Rule     | Value          |
		| GlobalTitle | Contains | apprenticeship |
	Given I navigated to the TraineeshipSearchPage page
	Then I see
		| Field       | Rule     | Value       |
		| GlobalTitle | Contains | traineeship |
	When I choose SignInLink
	And I am on the LoginPage page
	Then I see
		| Field       | Rule     | Value       |
		| GlobalTitle | Contains | traineeship |