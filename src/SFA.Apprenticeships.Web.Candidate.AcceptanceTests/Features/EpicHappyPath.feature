@ignore
Feature: EpicHappyPath
	In order to test the epic happy path
	As a candidate
	I want to be able to search, view, register, login, save, apply, view, resume and dismiss applications

Scenario: Epic happy path
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
