@US583
Feature: WarningWhenNavigatingOutOfApplyForTraineeshipPage
	As a candidate who is navigating away from the traineeship application form
	I want to be warned that I will lose changes if I proceed
	so that I don't lose any changes I have made without consciously deciding to do so

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

Scenario: Selecting No I will remain in the same page
	Given I have registered a new candidate
	When I select the "first" traineeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the TraineeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the TraineeshipApplicationPage page
	When I choose QualificationsYes
	And I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the TraineeshipApplicationPage page
	When I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I choose MyApplicationsLink
	When I see an alert box and select No
	When I am on the TraineeshipApplicationPage page
	Then I am on the TraineeshipApplicationPage page
	#This is required cleanup for this test. Without it the next test run will fail when reusing the browser
	When I choose MyApplicationsLink
	And I see an alert box and select Yes
	Then I am on the MyApplicationsPage page

Scenario: Selecting yes will redirect me to another page
	Given I have registered a new candidate
	When I select the "first" traineeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the TraineeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the TraineeshipApplicationPage page
	When I choose QualificationsYes
	And I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the TraineeshipApplicationPage page
	When I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I choose MyApplicationsLink
	When I see an alert box and select Yes
	When I am on the MyApplicationsPage page
	Then I am on the MyApplicationsPage page
