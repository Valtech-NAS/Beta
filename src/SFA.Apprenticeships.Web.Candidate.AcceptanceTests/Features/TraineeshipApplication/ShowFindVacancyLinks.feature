Feature: Dashboard find vacancy links
	As a candidate 
	I want links to find relevant vacancies

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@US586
Scenario: I have applied for neither traineeships nor apprenticeships
	Given I have an empty dashboard
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                    | Rule           | Value |
		| FindApprenticeshipLink   | Does Not Exist |       |
		| FindTraineeshipLink      | Does Not Exist |       |
		| FindApprenticeshipButton | Exists         |       |
		| TraineeshipsPrompt       | Does Not Exist |       |
	When I am on the MyApplicationsPage page
	And I choose MySettingsLink
	Then I am on the SettingsPage page
	And I see
		| Field                  | Rule           | Value |
		| FindApprenticeshipLink | Exists         |       |
		| FindTraineeshipLink    | Does Not Exist |       |
		
@US586
Scenario: I have applied for one or more apprenticeship and no traineeships
	Given I have an empty dashboard
	And I add 2 applications in "Draft" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                    | Rule           | Value |
		| FindApprenticeshipLink   | Exists         |       |
		| FindTraineeshipLink      | Does Not Exist |       |
		| FindApprenticeshipButton | Does Not Exist |       |
		| TraineeshipsPrompt       | Does Not Exist |       |
	When I am on the MyApplicationsPage page
	And I choose MySettingsLink
	Then I am on the SettingsPage page
	And I see
		| Field                  | Rule           | Value |
		| FindApprenticeshipLink | Exists         |       |
		| FindTraineeshipLink    | Does Not Exist |       |

@US586
Scenario: I have applied for six or more unsuccessful apprenticeships and no traineeships
	Given I have an empty dashboard
	And I add 6 applications in "Unsuccessful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                    | Rule           | Value |
		| FindApprenticeshipLink   | Exists         |       |
		| FindTraineeshipLink      | Exists         |       |
		| FindApprenticeshipButton | Does Not Exist |       |
		| TraineeshipsPrompt       | Exists         |       |
	When I am on the MyApplicationsPage page
	And I choose MySettingsLink
	Then I am on the SettingsPage page
	And I see
		| Field                  | Rule   | Value |
		| FindApprenticeshipLink | Exists |       |
		| FindTraineeshipLink    | Exists |       |

@US586
Scenario: I have applied for six or more unsuccessful apprenticeships one successful apprenticeship and no traineeships
	Given I have an empty dashboard
	And I add 6 applications in "Unsuccessful" state
	And I add 6 applications in "Successful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                    | Rule           | Value |
		| FindApprenticeshipLink   | Exists         |       |
		| FindTraineeshipLink      | Exists         |       |
		| FindApprenticeshipButton | Does Not Exist |       |
		| TraineeshipsPrompt       | Does Not Exist |       |
	When I am on the MyApplicationsPage page
	And I choose MySettingsLink
	Then I am on the SettingsPage page
	And I see
		| Field                  | Rule   | Value |
		| FindApprenticeshipLink | Exists |       |
		| FindTraineeshipLink    | Exists |       |

@US586
Scenario: I have applied for one or more traineeships and no apprenticeships
	Given I have an empty dashboard
	And I applied for 1 traineeships
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
	And I choose SignInButton
	Then I am on the MyApplicationsPage page
	And I see
		| Field                    | Rule           | Value |
		| FindApprenticeshipLink   | Exists         |       |
		| FindTraineeshipLink      | Exists         |       |
		| FindApprenticeshipButton | Does Not Exist |       |
		| TraineeshipsPrompt       | Does Not Exist |       |
	When I am on the MyApplicationsPage page
	And I choose MySettingsLink
	Then I am on the SettingsPage page
	And I see
		| Field                  | Rule   | Value |
		| FindApprenticeshipLink | Exists |       |
		| FindTraineeshipLink    | Exists |       |

@US658
Scenario: I am not interested in traineeships and I want to dismiss the traineeships prompt
	Given I have an empty dashboard
	And I add 6 applications in "Unsuccessful" state
	And I navigated to the LoginPage page
	When I am on the LoginPage page
	And I enter data
		| Field        | Value               |
		| EmailAddress | {EmailAddressToken} |
		| Password     | {PasswordToken}     |
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
