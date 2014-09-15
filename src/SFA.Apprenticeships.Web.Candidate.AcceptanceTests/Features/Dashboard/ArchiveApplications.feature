Feature: ArchiveApplications
	As a candidate,
	I want to be able to view the status of my application(s)
	so that I can track the progress of my application(s) 

Background: 
	Given I navigated to the HomePage page
	When I am on the HomePage page

@ignore
@US154
Scenario: As an candidate I want to be able to delete draft applications 
	Given I have an empty dashboard
	And I add 2 applications in "Successful" state
	And I add 2 applications in "Unsuccessful" state
	#think about the link
	When I choose DeleteLink 
	Then I am on the MyApplicationsPage page
	And I see
		| Field                         | Rule   | Value |
		| SuccessfulApplicationsCount   | Equals | 1     |
		| UnsuccessfulApplicationsCount | Equals | 1     |
