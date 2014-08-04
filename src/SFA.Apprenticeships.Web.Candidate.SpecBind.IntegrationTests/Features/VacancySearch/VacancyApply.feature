@US447
Feature: Vacancy Details pre and post sign-in behaviour
	As a candidate I want to be taken to correct place and
	be notified depending on various scenarios to be defined

Scenario: Candidate is not signed-in
	Given I am signed out
	And I am on the VacancyDetailsPage page for a vacancy that is live
	Then I must sign-in to apply for the vacancy 
