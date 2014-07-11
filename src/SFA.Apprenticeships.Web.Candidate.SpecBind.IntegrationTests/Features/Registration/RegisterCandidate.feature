Feature: Register Candidate
	In order to apply for a vacnacy
	As an apprentice
	I want to be able to register for the service

@mytag
Scenario: As a candidate I am on the registration page and all required fields are present
	Given I navigated to the RegisterCandidatePage page
	When I am on the RegisterCandidatePage page
	Then I wait to see Firstname
	And I wait to see Lastname
