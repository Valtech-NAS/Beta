Feature: PrePopulate
	In order to speed up the application process
	As a candidate
	I want valid data I have previously entered to pre populate the application form

Background: 
	Given I navigated to the HomePage page
	And I am logged out
	And I navigated to the HomePage page
	Then I am on the HomePage page

@US461
Scenario: Pre-populate my personal and contact details
	Given I have registered a new candidate
	When I select the first vacancy in location "N7 8LS" that can apply by this website
	Then I am on the VacancyDetailsPage page
	When I choose ApplyButton
	Then I am on the ApplicationPage page
