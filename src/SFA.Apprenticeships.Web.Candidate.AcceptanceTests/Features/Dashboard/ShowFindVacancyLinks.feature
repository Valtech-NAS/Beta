Feature: Dashboard find vacancy links
	As a candidate 
	I want links to find relevant vacancies

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@US658
Scenario: I am not interested in traineeships and I want to dismiss the traineeships prompt
	Given I have an empty dashboard
	And I add 6 applications in "Unsuccessful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value           |
		| EmailAddress | {EmailToken}    |
		| Password     | {PasswordToken} |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                    | Rule           | Value |
		| FindApprenticeshipLink   | Exists         |       |
		| FindTraineeshipLink      | Exists         |       |
		| TraineeshipsPrompt       | Exists         |       |
	When I choose DismissTraineeshipPromptsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                  | Rule           | Value |
		| FindApprenticeshipLink | Exists         |       |
		| FindTraineeshipLink    | Exists         |       |
		| TraineeshipsPrompt     | Does Not Exist |       |
	When I am on the MyApplicationsPage page
	And I choose MySettingsLink
	Then I am on the SettingsPage page
	And I see
		| Field                  | Rule   | Value |
		| FindApprenticeshipLink | Exists |       |
		| FindTraineeshipLink    | Exists |       |
