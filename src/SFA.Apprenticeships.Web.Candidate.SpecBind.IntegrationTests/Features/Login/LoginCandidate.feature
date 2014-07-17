@US415
Feature: Login Candidate
	As a candidate,
	I want to sign in so that I can access my profile,
	apply for apprenticeships etc.

Scenario: As a candidate all required fields are present
	Given I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	Then I wait to see EmailAddress
	And I wait to see Password

Scenario: As a candidate I must provide an email address and password
	Given I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	When I choose SignInButton

Scenario: As a candidate I can login with a registered and activated email address and password
	Given I navigated to the LoginCandidatePage page
	When I am on the LoginCandidatePage page
	And I enter data
		| Field			| Value			|
		| EmailAddress	| valtechnas@gmail.com	|
		| Password		| ?Password01!		|
	And I choose SignInButton
	Then I am on the VacancySearchPage page
