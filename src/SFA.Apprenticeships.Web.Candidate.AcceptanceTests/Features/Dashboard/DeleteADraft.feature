Feature: DeleteADraft
	As an candidate who has one or more active draft applications 
	I want to be able to delete draft applications 
	so that I can remove any applications that I no longer need.

Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@ignore
@US464
Scenario: As an candidate I want to be able to delete draft applications 
	Given I have an empty dashboard
	And I add 2 applications in "Draft" state
	When I choose DeleteLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                         | Rule   | Value |
		| DraftApplicationsCount        | Equals | 1     |
